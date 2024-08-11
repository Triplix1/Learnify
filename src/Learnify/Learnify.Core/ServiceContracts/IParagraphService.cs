using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.ServiceContracts;

public interface IParagraphService
{
    Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest, int authorId);
    Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphCreateRequest paragraphCreateRequest, int authorId);
    Task<ApiResponse> DeleteAsync(int id, int authorId);
}