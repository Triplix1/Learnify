using System.Linq.Expressions;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Custom;

public class CourseOnlyPublishedSpecification: Specification<Course>
{
    public override Expression<Func<Course, bool>> GetExpression()
    {
        return s => s.IsPublished == true;
    }
}