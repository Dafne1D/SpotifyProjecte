using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Domain.Entities;

namespace SpotifyAPI.Infrastructure.Mappers;

public static class UserRoleMapper
{

    public static UserRole ToDomain(UserRoleEntity userRoleEntity)
        => new UserRole(
            userRoleEntity.UserId,
            userRoleEntity.RoleId
        );

    public static UserRoleEntity ToEntity(UserRole userRole, Guid id)
        => new UserRoleEntity
        {
            Id = id,
            UserId = userRole.UserId,
            RoleId = userRole.RoleId
        };

        
}