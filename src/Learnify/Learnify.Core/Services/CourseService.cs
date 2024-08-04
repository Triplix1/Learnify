using AutoMapper;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

/// <inheritdoc />
public class CourseService: ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="mapper"></param>
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync()
    {
        var courses = await _unitOfWork.CourseRepository.GetAllAsync();
        return ApiResponse<IEnumerable<CourseResponse>>.Success(_mapper.Map<IEnumerable<CourseResponse>>(courses));
    }

    public async Task<ApiResponse<CourseResponse>> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}