using System.Security.Permissions;
using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record RoleRequest(string Name, string Description)
{
    public Role ToRole(Guid id, string code)
    {
        return new Role
        {
            Id = id,
            Code  = code,
            Name = Name,
            Description = Description
        };
    }
}