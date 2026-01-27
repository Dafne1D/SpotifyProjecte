using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record PermissionRequest(string Name, string Description)
{
    public Permission ToPermission(Guid id, string code)
    {
        return new Permission
        {
            Id = id,
            Code = code,
            Name = Name,
            Description = Description
        };
    }
}