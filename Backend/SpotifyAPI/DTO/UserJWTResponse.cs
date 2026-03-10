using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record UserJWTResponse(Guid Id, string Email, string Password, List<string> Roles)
{
    public static UserJWTResponse FromUser(Guid id, string email, string password, List<string> roles)
    {
        return new UserJWTResponse(id, email, password, roles);
    }
}
