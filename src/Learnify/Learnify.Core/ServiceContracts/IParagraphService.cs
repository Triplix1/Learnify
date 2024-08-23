using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.ServiceContracts;

public interface IParagraphService
{
    Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest, int userId);
    Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphUpdateRequest paragraphCreateRequest, int userId);
    Task<ApiResponse> DeleteAsync(int id, int userId);
}