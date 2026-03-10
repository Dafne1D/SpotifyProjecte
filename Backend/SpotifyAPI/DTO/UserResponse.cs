using SpotifyAPI.Infrastructure.Mappers;
using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Domain.Entities;

namespace SpotifyAPI.DTO;

public record UserResponse(Guid Id, string Username, string Email)
{
    public static UserResponse FromUser(User user, Guid id)
    {
        return new UserResponse(id, user.Username, user.Email);
    }
}
