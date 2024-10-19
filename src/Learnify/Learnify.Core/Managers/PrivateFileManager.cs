using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Managers;

public class PrivateFileManager : IPrivateFileManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public PrivateFileManager(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var privateFile = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id, cancellationToken);

        using var ts = TransactionScopeBuilder.CreateReadCommittedAsync();

        var deleted = await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(id, cancellationToken);

        if (deleted) await _blobStorage.DeleteAsync(privateFile.ContainerName, privateFile.BlobName, cancellationToken);

        ts.Complete();
    }

    public async Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var privateFiles = await _psqUnitOfWork.PrivateFileRepository.GetByIdsAsync(ids, cancellationToken);

        using var ts = TransactionScopeBuilder.CreateReadCommittedAsync();

        var deleted = await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(ids, cancellationToken);

        if (deleted)
            foreach (var privateFile in privateFiles)
                await _blobStorage.DeleteAsync(privateFile.ContainerName, privateFile.BlobName, cancellationToken);

        ts.Complete();
    }
}