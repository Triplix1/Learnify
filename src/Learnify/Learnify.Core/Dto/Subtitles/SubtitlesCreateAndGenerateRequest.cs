using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Subtitles;

public class SubtitlesCreateAndGenerateRequest
{
    public string VideoBlobName { get; set; }   
    public string VideoContainerName { get; set; }  
    public string LessonId { get; set; }
    public IEnumerable<Language> SubtitlesLanguages { get; set; }
    public Language PrimaryLanguage { get; set; }
    public int? CourseId { get; set; }
}