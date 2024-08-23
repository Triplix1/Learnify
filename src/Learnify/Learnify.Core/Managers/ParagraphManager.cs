using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class ParagraphManager: IParagraphManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;

    public ParagraphManager(IPsqUnitOfWork psqUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
    }

    public async Task<Exception> ValidateAuthorOfParagraphAsync(int paragraphId, int userId)
    {
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(paragraphId);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");
            
        if(authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }
}