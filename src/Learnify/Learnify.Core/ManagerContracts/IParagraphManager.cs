namespace Learnify.Core.ManagerContracts;

public interface IParagraphManager
{
    Task<Exception> ValidateExistAndAuthorOfParagraphAsync(int paragraphId, int userId);
}