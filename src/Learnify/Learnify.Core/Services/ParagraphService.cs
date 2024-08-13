using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class ParagraphService: IParagraphService
{
    public async Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest, int authorId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphCreateRequest paragraphCreateRequest, int authorId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> DeleteAsync(int id, int authorId)
    {
        throw new NotImplementedException();
    }
}