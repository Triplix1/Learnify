using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class ParagraphService : IParagraphService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;

    public ParagraphService(IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IMongoUnitOfWork mongoUnitOfWork,
        ICourseService courseService)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _mongoUnitOfWork = mongoUnitOfWork;
        _courseService = courseService;
    }

    #region DML

    public async Task<ParagraphResponse> CreateAsync(ParagraphCreateRequest paragraphCreateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(paragraphCreateRequest.CourseId, userId,
            cancellationToken);

        var paragraph = _mapper.Map<Paragraph>(paragraphCreateRequest);
        paragraph.IsPublished = false;

        paragraph = await _psqUnitOfWork.ParagraphRepository.CreateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return response;
    }

    public async Task<ParagraphResponse> UpdateAsync(ParagraphUpdateRequest paragraphUpdateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        var originalParagraph =
            await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(paragraphUpdateRequest.Id,
                cancellationToken: cancellationToken);

        if (originalParagraph is null)
            throw new KeyNotFoundException("cannot find paragraph with such id");

        await _userAuthorValidatorManager.ValidateAuthorOfParagraphAsync(paragraphUpdateRequest.Id, userId,
            cancellationToken);

        var paragraph = _mapper.Map(paragraphUpdateRequest, originalParagraph);

        paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return response;
    }

    public async Task<ParagraphPublishedResponse> PublishAsync(PublishParagraphRequest publishParagraphRequest,
        int userId,
        CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfParagraphAsync(publishParagraphRequest.ParagraphId, userId,
            cancellationToken);

        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(publishParagraphRequest.ParagraphId,
            cancellationToken: cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with specified id");

        var unpublishedCourse = false;

        if (publishParagraphRequest.Publish)
        {
            await ValidateParagraphAsync(paragraph, cancellationToken);
        }

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            paragraph.IsPublished = publishParagraphRequest.Publish;

            paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

            if (!paragraph.IsPublished)
            {
                var amountOfPublishedParagraphsPerCourse =
                    await _psqUnitOfWork.ParagraphRepository.GetAmountOfPublishedParagraphsPerCourseAsync(
                        paragraph.CourseId,
                        cancellationToken);

                if (amountOfPublishedParagraphsPerCourse == 0)
                {
                    var publishCourseRequest = new PublishCourseRequest()
                    {
                        CourseId = paragraph.CourseId,
                        Publish = false
                    };

                    await _courseService.PublishAsync(publishCourseRequest, userId, cancellationToken);

                    unpublishedCourse = true;
                }
            }
            
            transaction.Complete();
        }

        return new ParagraphPublishedResponse()
        {
            UnpublishedCourse = unpublishedCourse
        };
    }

    public async Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such id");

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(paragraph.CourseId, userId, cancellationToken);

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            await _mongoUnitOfWork.Lessons.DeleteForParagraphAsync(id, cancellationToken);

            var deletionResult = await _psqUnitOfWork.ParagraphRepository.DeleteAsync(id, cancellationToken);

            if (!deletionResult)
                throw new Exception("Cannot delete paragraph with such id");

            transaction.Complete();
        }
    }

    #endregion

    #region Private

    private async Task ValidateParagraphAsync(Paragraph paragraph, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(paragraph.Name))
        {
            errors.Add("Навза розділу обов'язкова");
        }

        var amountOfPublishedLessonsPerParagraph =
            await _mongoUnitOfWork.Lessons.GetAmountOfPublishedLessonsPerParagraph(paragraph.Id, cancellationToken);

        if (amountOfPublishedLessonsPerParagraph == 0)
        {
            errors.Add("Опублікований розділ повинен містити принаймні один збережений урок");
        }

        if (errors.Count > 0)
        {
            throw new CompositeException(errors);
        }
    }

    #endregion
}