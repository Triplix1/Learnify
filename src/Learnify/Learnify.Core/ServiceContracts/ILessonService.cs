using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.ServiceContracts;

public interface ILessonService
{
    Task<ApiResponse> DeleteAsync(string id, int userId);
    Task<ApiResponse<IEnumerable<LessonTitleResponse>>> GetByParagraphAsync(int paragraphId); 
    Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId);
    Task<ApiResponse<LessonUpdateResponse>> AddOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId);
    Task<ApiResponse<LessonUpdateResponse>> SaveDraftAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId);
    Task<ApiResponse<LessonResponse>> GetByIdAsync(string id, int userId);
}