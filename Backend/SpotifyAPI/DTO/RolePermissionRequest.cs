using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record RolePermissionRequest(Guid RoleId, Guid PermissionId)
{
    public RolePermission ToRolePermission(Guid id)
    {
        return new RolePermission
        {
            Id = id,
            RoleId = RoleId,
            PermissionId = PermissionId
        };
    }
}