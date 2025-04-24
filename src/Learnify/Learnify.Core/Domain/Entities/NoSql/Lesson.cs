using Learnify.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

public class Lesson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int ParagraphId { get; set; }
    public string Title { get; set; }
    public string OriginalLessonId { get; set; }
    public string EditedLessonId { get; set; }
    public bool IsDraft { get; set; }
    public Language PrimaryLanguage { get; set; }
    
    public Video Video { get; set; }

    public IList<QuizQuestion> Quizzes { get; set; } = [];
}