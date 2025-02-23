using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using MassTransit;

namespace Learnify.Core.Consumers;

public class FileTranslatedConsumer: IConsumer<TranslatedResponse>
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;

    public FileTranslatedConsumer(IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<TranslatedResponse> context)
    {
        var message = context.Message;
        var fileData = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(message.FileId);
        
        _mapper.Map(message.FileInfo, fileData);
        await _psqUnitOfWork.PrivateFileRepository.UpdateFileAsync(fileData);
    }
}