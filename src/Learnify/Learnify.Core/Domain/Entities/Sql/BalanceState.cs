namespace Learnify.Core.Domain.Entities.Sql;

public class BalanceState
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public decimal TotalAccumulated { get; set; }
    public decimal Paid { get; set; }
    public Course Course { get; set; }
    IEnumerable<int> Payments { get; set; }
}