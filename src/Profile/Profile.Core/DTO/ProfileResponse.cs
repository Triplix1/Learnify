using Profile.Core.Domain.Entities;

namespace Profile.Core.DTO;

public class ProfileResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public UserType Type { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? CardNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public string? PhotoPublicId { get; set; }
}