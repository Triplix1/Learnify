using JetBrains.Annotations;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.Subtitles;

public class SubtitlesResponse
{
    [CanBeNull]
    public AttachmentResponse File { get; set; }
    public Language Language;
}