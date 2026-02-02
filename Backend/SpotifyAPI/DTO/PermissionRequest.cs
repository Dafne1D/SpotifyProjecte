using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record PermissionRequest(string Name, string Description)
{
    public Permission ToPermission(Guid id)
    {
        return new Permission
        {
            Id = id,
            Name = Name,
            Description = Description
        };
    }
}