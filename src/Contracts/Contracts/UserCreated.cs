namespace Contracts;

public class UserCreated
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? CardNumber { get; set; }
}