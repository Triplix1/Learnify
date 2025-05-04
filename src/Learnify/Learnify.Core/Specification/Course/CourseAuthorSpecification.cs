using System.Linq.Expressions;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Course;

public class CourseAuthorSpecification: Specification<Domain.Entities.Sql.Course>
{
    public int AuthorId { get; set; }
    
    public CourseAuthorSpecification(int authorId)
    {
        AuthorId = authorId;
    }
    public override Expression<Func<Domain.Entities.Sql.Course, bool>> GetExpression()
    {
        return course => course.AuthorId == AuthorId;
    }
}