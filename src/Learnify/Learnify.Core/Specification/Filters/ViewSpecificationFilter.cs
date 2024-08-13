using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Enums;
using Learnify.Core.Specification.Base;
using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification.Filters;

public class ViewSpecificationFilter: ISpecificationFilter<View>
{
    public Specification<View> Specification { get; set; }
    public required TimeDateRanges DateRanges { get; set; }
}