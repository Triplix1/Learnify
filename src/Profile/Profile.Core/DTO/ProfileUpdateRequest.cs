using System.ComponentModel.DataAnnotations;
using Profile.Core.Domain.Entities;

namespace Profile.Core.DTO;

public class ProfileUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? CardNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public string? PhotoPublicId { get; set; }
}