using SpotifyAPI.Infrastructure.Mappers;
using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Domain.Entities;
namespace SpotifyAPI.DTO;

public record UserRequest(string Username, string Email, string Password)
{
public User ToUser(string hash, string salt)
{
    return new User(

            Username, 
            Email, 
            hash, 
            salt
        );
    }
}
