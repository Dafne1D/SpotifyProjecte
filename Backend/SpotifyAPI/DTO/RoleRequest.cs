using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record RoleRequest(string Name, string Description)
{
    public Role ToRole(Guid id)
    {
        return new Role
        {
            Id = id,
            Name = Name,
            Description = Description
        };
    }
}