using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class ParagraphService : IParagraphService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IMapper _mapper;
    private readonly IPublishingService _publishingService;
    private readonly ILessonService _lessonService;

    public ParagraphService(IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IPublishingService publishingService,
        ILessonService lessonService)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _publishingService = publishingService;
        _lessonService = lessonService;
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

    public async Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such id");

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(paragraph.CourseId, userId, cancellationToken);

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            await _lessonService.DeleteByParagraphAsync(id, userId, cancellationToken);

            var deletionResult = await _psqUnitOfWork.ParagraphRepository.DeleteAsync(id, cancellationToken);

            await _publishingService.HandleParagraphUnpublishing(userId, cancellationToken, paragraph);
            
            if (!deletionResult)
                throw new Exception("Cannot delete paragraph with such id");

            transaction.Complete();
        }
    }

    #endregion
}