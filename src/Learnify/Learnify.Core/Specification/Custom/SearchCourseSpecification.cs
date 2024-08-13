using System.Linq.Expressions;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Custom;

public class SearchCourseSpecification: Specification<Course>
{
    public string SearchString { get; set; }
    
    public SearchCourseSpecification(string searchString)
    {
        SearchString = searchString;
    }
    public override Expression<Func<Course, bool>> GetExpression()
    {
        return course => course.Name.Contains(SearchString);
    }
}