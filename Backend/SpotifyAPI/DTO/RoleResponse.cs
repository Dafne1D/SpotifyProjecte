using SpotifyAPI.Model;

public record RoleResponse(Guid Id, string Name, string Description)
{
    public static RoleResponse FromRole(Role role)
    {
        return new RoleResponse(role.Id, role.Name, role.Description);
    }
}