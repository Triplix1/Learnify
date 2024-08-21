using Learnify.Core.Dto.Attachment;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.Video;

public class VideoAddOrUpdateRequest
{
    public AttachmentResponse Attachment { get; set; }
    public Language PrimaryLanguage { get; set; }
    public IEnumerable<Language> Subtitles { get; set; }
}