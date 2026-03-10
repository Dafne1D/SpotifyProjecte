namespace SpotifyAPI.Infrastructure.Persistence.Entities;

public class UserRoleEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}