using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace AuthIdentity.Infrastructure.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{

    public UserRepository(IdentityDbContext context) : base(context)
    {
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}