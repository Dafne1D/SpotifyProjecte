using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record RoleResponse(Guid Id, string Name, string Description)
{
    public static RoleResponse FromRole(Role role)
    {
        return new RoleResponse(role.Id, role.Name, role.Description);
    }
}