using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class PublishingService: IPublishingService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public PublishingService(IPsqUnitOfWork psqUnitOfWork,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IMongoUnitOfWork mongoUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task PublishCourseAsync(PublishCourseRequest publishCourseRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(publishCourseRequest.CourseId, userId,
            cancellationToken);

        var courseValidationResponse =
            await _psqUnitOfWork.CourseRepository.GetCourseValidationResponse(publishCourseRequest.CourseId,
                cancellationToken);

        if(publishCourseRequest.Publish)
            ValidateBeforePublishingCourse(courseValidationResponse);

        var success = await _psqUnitOfWork.CourseRepository.PublishAsync(publishCourseRequest.CourseId,
            publishCourseRequest.Publish, cancellationToken);

        if (!success)
            throw new Exception("Course Publish Failed");
    }
    
    public async Task HandleParagraphUnpublishing(int userId, CancellationToken cancellationToken, Paragraph paragraph)
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

            await PublishCourseAsync(publishCourseRequest, userId, cancellationToken);
        }
    }

    public async Task<ParagraphPublishedResponse> PublishParagraphAsync(PublishParagraphRequest publishParagraphRequest,
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
                await HandleParagraphUnpublishing(userId, cancellationToken, paragraph);
                
                unpublishedCourse = true;
            }
            
            transaction.Complete();
        }

        return new ParagraphPublishedResponse()
        {
            UnpublishedCourse = unpublishedCourse
        };
    }

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

    private void ValidateBeforePublishingCourse(CourseValidationResponse course)
    {
        var errors = new List<string>();

        if (course.PhotoId is null)
        {
            errors.Add("Фото курсу є обов'язковим");
        }

        if (course.VideoId is null)
        {
            errors.Add("Відео курсу є обов'язковим");
        }

        var amountOfPublishedCourses = course.Paragraphs?.Where(p => p.IsPublished).Count() ?? 0;

        if (amountOfPublishedCourses == 0)
        {
            errors.Add("Курс повинен містити мінімум один опубліковний розділ");
        }

        if (errors.Any())
        {
            throw new CompositeException(errors);
        }
    }

}