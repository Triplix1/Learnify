using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// UserRepository
/// </summary>
public class UserRepository: BaseRepository<User, int>, IUserRepository
{

    /// <summary>
    /// Initializes a new instance of <see cref="UserRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}