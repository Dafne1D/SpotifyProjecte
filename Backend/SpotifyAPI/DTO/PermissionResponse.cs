using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record PermissionResponse(Guid Id, string Code, string Name, string? Description)
{
    public static PermissionResponse FromPermission(Permission permission)
    {
        return new PermissionResponse(permission.Id, permission.Code, permission.Name, permission.Description);
    }
}
