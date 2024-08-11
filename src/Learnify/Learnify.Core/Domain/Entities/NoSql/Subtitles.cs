using JetBrains.Annotations;
using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.NoSql;

public class Subtitles
{
    [CanBeNull]
    public Attachment File { get; set; }
    public Language Language;
}