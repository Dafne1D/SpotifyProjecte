using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record UserRoleRequest(Guid UserId, Guid RoleId)
{
    public UserRole ToUserRole(Guid id)
    {
        return new UserRole
        {
            Id = id,
            UserId = UserId,
            RoleId = RoleId
        };
    }
}