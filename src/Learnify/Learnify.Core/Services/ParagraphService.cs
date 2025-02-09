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
    private readonly IUserAuthorValidatorManager _iUserAuthorValidatorManager;
    private readonly IMapper _mapper;

    public ParagraphService(IPsqUnitOfWork psqUnitOfWork, IMapper mapper, IUserAuthorValidatorManager iUserAuthorValidatorManager)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _iUserAuthorValidatorManager = iUserAuthorValidatorManager;
    }

    public async Task<ParagraphResponse> CreateAsync(ParagraphCreateRequest paragraphCreateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        await _iUserAuthorValidatorManager.ValidateAuthorOfCourseAsync(paragraphCreateRequest.CourseId, userId,
                cancellationToken);

        var paragraph = _mapper.Map<Paragraph>(paragraphCreateRequest);
        paragraph.isPublished = false;

        paragraph = await _psqUnitOfWork.ParagraphRepository.CreateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return response;
    }

    public async Task<ParagraphResponse> UpdateAsync(ParagraphUpdateRequest paragraphUpdateRequest,
        int userId, CancellationToken cancellationToken = default)
    {
        var originalParagraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(paragraphUpdateRequest.Id, cancellationToken: cancellationToken);
        
        if(originalParagraph is null)
            throw new KeyNotFoundException("cannot find paragraph with such id");
        
        await _iUserAuthorValidatorManager.ValidateAuthorOfParagraphAsync(paragraphUpdateRequest.Id, userId,
                cancellationToken);

        var paragraph = _mapper.Map(paragraphUpdateRequest, originalParagraph);

        paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return response;
    }

    public async Task<ParagraphResponse> PublishAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default)
    {
        await _iUserAuthorValidatorManager.ValidateAuthorOfParagraphAsync(paragraphId, userId, cancellationToken);

        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(paragraphId, cancellationToken: cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with specified id");

        paragraph.isPublished = true;

        paragraph = await _psqUnitOfWork.ParagraphRepository.UpdateAsync(paragraph, cancellationToken);

        var response = _mapper.Map<ParagraphResponse>(paragraph);

        return response;
    }

    public async Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such id");

        await _iUserAuthorValidatorManager.ValidateAuthorOfCourseAsync(paragraph.CourseId, userId, cancellationToken);

        var deletionResult = await _psqUnitOfWork.ParagraphRepository.DeleteAsync(id, cancellationToken);

        if (!deletionResult)
            throw new Exception("Cannot delete paragraph with such id");
    }
}