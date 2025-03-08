using Learnify.Contracts;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.File;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;
using MassTransit;

namespace Learnify.Core.Managers;

public class SummaryManager : ISummaryManager
{
    private readonly IPrivateFileRepository _privateFileRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public SummaryManager(IPublishEndpoint publishEndpoint,
        IPrivateFileRepository privateFileRepository)
    {
        _publishEndpoint = publishEndpoint;
        _privateFileRepository = privateFileRepository;
    }

    public async Task<int> GenerateSummaryForVideoAsync(PrivateFileDataBlobResponse videoBlobResponse, int? courseId,
        Language primaryLanguage)
    {
        var summaryFileCreateRequest = new PrivateFileCreateRequest()
        {
            CourseId = courseId
        };

        var summaryFileResponse = await _privateFileRepository.CreateFileAsync(summaryFileCreateRequest);

        var generateRequest = new SummaryGenerateRequest()
        {
            FileId = summaryFileResponse.Id,
            Language = primaryLanguage.ToString(),
            ContentType = videoBlobResponse.ContentType,
            VideoBlobName = videoBlobResponse.BlobName,
            VideoContainerName = videoBlobResponse.ContainerName,
        };
        
        await _publishEndpoint.Publish(generateRequest);

        return summaryFileResponse.Id;
    }
}