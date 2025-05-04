using System.Linq.Expressions;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Course;

public class SearchCourseSpecification: Specification<Domain.Entities.Sql.Course>
{
    public string SearchString { get; set; }
    
    public SearchCourseSpecification(string searchString)
    {
        SearchString = searchString;
    }
    public override Expression<Func<Domain.Entities.Sql.Course, bool>> GetExpression()
    {
        return course => course.Name.Contains(SearchString);
    }
}