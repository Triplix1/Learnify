namespace Learnify.Core.ManagerContracts;

public interface IParagraphManager
{
    Task<Exception> ValidateAuthorOfParagraphAsync(int paragraphId, int userId);
}