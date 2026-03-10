using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Domain.Entities;

namespace SpotifyAPI.Infrastructure.Mappers;

public static class UserMapper
{

    public static User ToDomain(UserEntity userEntity)
        => new User(
            userEntity.Username,
            userEntity.Email,
            userEntity.Password,
            userEntity.Salt
        );

    public static UserEntity ToEntity(User user, Guid id)
        => new UserEntity
        {
            Id = id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            Salt = user.Salt
        };

        
}