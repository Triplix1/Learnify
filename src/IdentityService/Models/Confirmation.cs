using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public class Confirmation
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    [MaxLength(4)]
    public string Code { get; set; } 
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(3);
}