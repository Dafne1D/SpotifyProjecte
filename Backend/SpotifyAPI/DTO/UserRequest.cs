using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record UserRequest(string Username, string Email, string Password)
{
    public User ToUser(Guid id, string hash, string salt)
    {
        return new User
        {
            Id = id,
            Username = Username,
            Email = Email,
            Password = hash,
            Salt = salt
        };
    }
}
