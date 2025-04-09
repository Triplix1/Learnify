namespace Learnify.Core.Exceptions;

public class CompositeException : Exception
{
    public object Exceptions { get; set; }

    public CompositeException(object exceptions)
    {
        Exceptions = exceptions;
    }
}