using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Enums;

namespace Learnify.Core.Specification;

public class ViewFilter: IFilter<View>
{
    public Specification<View> Specification { get; set; }
    public required TimeDateRanges DateRanges { get; set; }
}