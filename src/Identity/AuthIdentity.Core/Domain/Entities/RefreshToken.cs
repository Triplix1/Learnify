namespace AuthIdentity.Core.Domain.Entities;

public class RefreshToken: BaseEntity
{
    public string Jwt { get; set; }
    public string Refresh { get; set; }
    public DateTime Expire { get; set; }
    public bool HasBeenUsed { get; set; }
}