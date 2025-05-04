namespace Learnify.Core.Domain.Entities.NoSql;

public class Answers: IEquatable<Answers>
{
    public IEnumerable<string> Options { get; set; }
    public int CorrectAnswer { get; set; }

    public bool Equals(Answers other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Options.SequenceEqual(other.Options) && CorrectAnswer == other.CorrectAnswer;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Answers)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Options, CorrectAnswer);
    }
}