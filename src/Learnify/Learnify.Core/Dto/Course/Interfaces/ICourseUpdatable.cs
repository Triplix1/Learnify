namespace Learnify.Core.Dto.Course.Interfaces;

public interface ICourseUpdatable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}