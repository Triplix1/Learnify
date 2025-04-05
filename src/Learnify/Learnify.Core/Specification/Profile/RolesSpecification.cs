using System.Linq.Expressions;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Enums;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Profile;

public class RolesSpecification: Specification<User>
{
    private readonly IEnumerable<Role> _roles;

    public RolesSpecification(Role role)
    {
        _roles = [role];
    }
    
    public RolesSpecification(IEnumerable<Role> roles)
    {
        _roles = roles;
    }
    
    public override Expression<Func<User, bool>> GetExpression()
    {
        return u => _roles.Contains(u.Role);
    }
}