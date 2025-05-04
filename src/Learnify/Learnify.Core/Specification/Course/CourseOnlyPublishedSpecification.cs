using System.Linq.Expressions;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Course;

public class CourseOnlyPublishedSpecification: Specification<Domain.Entities.Sql.Course>
{
    public override Expression<Func<Domain.Entities.Sql.Course, bool>> GetExpression()
    {
        return s => s.IsPublished == true;
    }
}