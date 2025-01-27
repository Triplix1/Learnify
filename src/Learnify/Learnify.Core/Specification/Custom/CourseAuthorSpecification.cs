using System.Linq.Expressions;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Custom;

public class CourseAuthorSpecification: Specification<Course>
{
    public int AuthorId { get; set; }
    
    public CourseAuthorSpecification(int authorId)
    {
        AuthorId = authorId;
    }
    public override Expression<Func<Course, bool>> GetExpression()
    {
        return course => course.AuthorId == AuthorId;
    }
}