using Learnify.Core.Dto.File;
using Learnify.Core.Enums;

namespace Learnify.Core.ManagerContracts;

public interface ISummaryManager
{
    Task<int> GenerateSummaryForVideoAsync(PrivateFileDataBlobResponse videoBlobResponse, int? courseId,
        Language primaryLanguage);
}