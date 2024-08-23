using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class ParagraphService: IParagraphService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly ICourseManager _courseManager;
    private readonly IParagraphManager _paragraphManager;

    public ParagraphService(IPsqUnitOfWork psqUnitOfWork, ICourseManager courseManager, IParagraphManager paragraphManager)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _courseManager = courseManager;
        _paragraphManager = paragraphManager;
    }

    public async Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest, int userId)
    {
        var validationResult = await _courseManager.ValidateAuthorOfCourseAsync(paragraphCreateRequest.CourseId, userId);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var result = await _psqUnitOfWork.ParagraphRepository.CreateAsync(paragraphCreateRequest);

        return ApiResponse<ParagraphResponse>.Success(result);
    }

    public async Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphUpdateRequest paragraphUpdateRequest, int userId)
    {
        var validationResult = await _paragraphManager.ValidateAuthorOfParagraphAsync(paragraphUpdateRequest.Id, userId);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var response = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraphUpdateRequest);

        return ApiResponse<ParagraphResponse>.Success(response);
    }

    public async Task<ApiResponse> DeleteAsync(int id, int userId)
    {
        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(id);

        if (paragraph is null)
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find paragraph with such id"));
        
        var validationResult = await _courseManager.ValidateAuthorOfCourseAsync(paragraph.CourseId, userId);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var deletionResult = await _psqUnitOfWork.ParagraphRepository.DeleteAsync(id);

        if (!deletionResult)
            return ApiResponse.Failure(new Exception("Cannot delete paragraph with such id"));
        
        return ApiResponse.Success();
    }
}