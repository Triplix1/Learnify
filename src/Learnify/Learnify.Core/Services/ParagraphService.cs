using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class ParagraphService : IParagraphService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly ICourseManager _courseManager;
    private readonly IParagraphManager _paragraphManager;
    private readonly IMapper _mapper;

    public ParagraphService(IPsqUnitOfWork psqUnitOfWork, ICourseManager courseManager,
        IParagraphManager paragraphManager, IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _courseManager = courseManager;
        _paragraphManager = paragraphManager;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _courseManager.ValidateAuthorOfCourseAsync(paragraphCreateRequest.CourseId, userId,
                cancellationToken);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var paragraph = _mapper.Map<Paragraph>(paragraphCreateRequest);
        paragraph.isPublished = false;

        paragraph = await _psqUnitOfWork.ParagraphRepository.CreateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return ApiResponse<ParagraphResponse>.Success(response);
    }

    public async Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphUpdateRequest paragraphUpdateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _paragraphManager.ValidateExistAndAuthorOfParagraphAsync(paragraphUpdateRequest.Id, userId,
                cancellationToken);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var paragraph = _mapper.Map<Paragraph>(paragraphUpdateRequest);

        paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return ApiResponse<ParagraphResponse>.Success(response);
    }

    public async Task<ApiResponse<ParagraphResponse>> PublishAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _paragraphManager.ValidateExistAndAuthorOfParagraphAsync(paragraphId, userId, cancellationToken);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(paragraphId, cancellationToken);

        if (paragraph is null)
            return ApiResponse<ParagraphResponse>.Failure(
                new KeyNotFoundException("Cannot find paragraph with specified id"));

        paragraph.isPublished = true;

        paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return ApiResponse<ParagraphResponse>.Success(response);
    }

    public async Task<ApiResponse> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(id, cancellationToken);

        if (paragraph is null)
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find paragraph with such id"));

        var validationResult =
            await _courseManager.ValidateAuthorOfCourseAsync(paragraph.CourseId, userId, cancellationToken);

        if (validationResult is not null)
            return ApiResponse<ParagraphResponse>.Failure(validationResult);

        var deletionResult = await _psqUnitOfWork.ParagraphRepository.DeleteAsync(id, cancellationToken);

        if (!deletionResult)
            return ApiResponse.Failure(new Exception("Cannot delete paragraph with such id"));

        return ApiResponse.Success();
    }
}