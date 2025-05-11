using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.ServiceContracts;

public interface IPublishingService
{
    Task<ParagraphPublishedResponse> PublishParagraphAsync(PublishParagraphRequest publishParagraphRequest,
        int userId,
        CancellationToken cancellationToken = default);

    Task PublishCourseAsync(PublishCourseRequest publishCourseRequest, int userId,
        CancellationToken cancellationToken = default);

    Task HandleParagraphUnpublishing(int userId, CancellationToken cancellationToken, Paragraph paragraph);
}