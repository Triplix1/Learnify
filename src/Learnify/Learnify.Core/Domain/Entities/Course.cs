using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities;

/// <summary>
/// Course entity
/// </summary>
public class Course
{
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets value for PrimaryLanguage
    /// </summary>
    public Language PrimaryLanguage { get; set; }

    /// <summary>
    /// Gets or sets value for Ratings
    /// </summary>
    public ICollection<CourseRating> Ratings { get; set; }

    /// <summary>
    /// Get course rating 
    /// </summary>
    /// <returns></returns>
    public double GetRating()
    {
        var count = (double)Ratings.Count;

        if (count == 0d)
            return 0d;
        
        return Math.Round(Ratings.Sum(r => r.Rate) / count, 2);
    }
}