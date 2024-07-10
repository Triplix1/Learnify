using System.ComponentModel.DataAnnotations;

namespace Profile.Core.Domain.Entities;

public class User
{
    [Required]
    [Key]
    public string Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public UserType Type { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? CardNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public string? PhotoPublicId { get; set; }
}