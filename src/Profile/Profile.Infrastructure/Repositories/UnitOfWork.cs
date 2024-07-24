using Profile.Core.Domain.RepositoryContracts;
using Profile.Infrastructure.Data;

namespace Profile.Infrastructure.Repositories;

/// <inheritdoc />
public class UnitOfWork: IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of <see cref="UnitOfWork"/>
    /// </summary>
    /// <param name="profileRepository"><see cref="IProfileRepository"/></param>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public UnitOfWork(IProfileRepository profileRepository, ApplicationDbContext context)
    {
        ProfileRepository = profileRepository;
        _context = context;
    }

    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc />
    public IProfileRepository ProfileRepository { get; }
    
    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}