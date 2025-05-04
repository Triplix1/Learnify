using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

public class QuizQuestion: IEquatable<QuizQuestion>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Question { get; set; }
    public Answers Answers { get; set; }

    public bool Equals(QuizQuestion other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Question == other.Question && Equals(Answers, other.Answers);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((QuizQuestion)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Question, Answers);
    }
}