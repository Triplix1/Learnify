namespace Learnify.Core.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SkipApiResponseAttribute : Attribute
{
}