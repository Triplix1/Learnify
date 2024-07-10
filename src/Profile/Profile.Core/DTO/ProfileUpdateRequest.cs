using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Profile.Core.DTO;

public class ProfileUpdateRequest
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? CardNumber { get; set; }
    public IFormFile? File { get; set; }
}