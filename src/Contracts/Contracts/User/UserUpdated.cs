namespace Contracts;

public class UserUpdated
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string ImageUrl { get; set; }
    public string Role { get; set; }
}