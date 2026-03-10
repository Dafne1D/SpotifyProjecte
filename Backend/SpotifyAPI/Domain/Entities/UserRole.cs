using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace SpotifyAPI.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId=userId;
        RoleId=roleId;
    }
}