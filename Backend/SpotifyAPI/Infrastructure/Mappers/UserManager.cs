using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Domain.Entities;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace dbdemo.Infrastructure.Mappers;

public static class UserMapper
{

    public static User ToDomain(UserEntity userEntity)
        => new User(
            userEntity.Id,
            userEntity.Username,
            userEntity.Email,
            userEntity.Password,
            userEntity.Salt
        );

    public static UserEntity ToEntity(User user, Guid id)
        => new User(
            Id = id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            Salt = user.Salt
        );

        
}