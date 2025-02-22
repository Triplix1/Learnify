using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.Video;

public class VideoResponse
{
    public AttachmentResponse Attachment { get; set; }
    public IEnumerable<SubtitlesResponse> Subtitles { get; set; }
}