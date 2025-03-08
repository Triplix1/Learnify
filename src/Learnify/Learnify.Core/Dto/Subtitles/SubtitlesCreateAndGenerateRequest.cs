using Learnify.Core.Dto.File;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Subtitles;

public class SubtitlesCreateAndGenerateRequest
{
    public PrivateFileDataBlobResponse PrivateFileDataBlobResponse { get; set; }  
    public string LessonId { get; set; }
    public IEnumerable<Language> SubtitlesLanguages { get; set; }
    public Language PrimaryLanguage { get; set; }
    public int? CourseId { get; set; }
}