using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.NoSql;

public class SubtitleReference
{
    public int SubtitleId { get; set; }
    public Language Language { get; set; }
}