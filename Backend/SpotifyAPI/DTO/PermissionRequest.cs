using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record PermissionRequest(string Code, string Name, string Description)
{
    public Permission ToPermission(Guid id)
    {
        return new Permission
        {
            Id = id,
            Code = Code,
            Name = Name,
            Description = Description
        };
    }
}