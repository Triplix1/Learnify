using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.ServiceContracts;

public interface IParagraphService
{
    Task<ParagraphResponse> CreateAsync(ParagraphCreateRequest paragraphCreateRequest, int userId,
        CancellationToken cancellationToken = default);

    Task<ParagraphResponse> UpdateAsync(ParagraphUpdateRequest paragraphCreateRequest, int userId,
        CancellationToken cancellationToken = default);

    Task<ParagraphResponse> PublishAsync(PublishParagraphRequest publishParagraphRequest, int userId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
}