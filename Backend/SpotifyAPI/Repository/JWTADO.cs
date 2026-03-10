using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.DTO;
using SpotifyAPI.Utils;

namespace SpotifyAPI.Repository;

static class JWTADO
{
    public static UserJWTResponse? GetByLogin(SpotifyDBConnection dbConn, string login)
    {
        dbConn.Open();
        string sql = @"SELECT u.Id, u.Email, u.Password, r.Name
                    FROM Users u
                    LEFT JOIN UserRoles ur ON u.Id = ur.UserId
                    LEFT JOIN Roles r ON ur.RoleId = r.Id
                    WHERE u.Email = @Login OR u.Username = @Login";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Login", login);

        using SqlDataReader reader = cmd.ExecuteReader();
        UserJWTResponse? user = null;
        List<string> roles = new List<string>();

        while (reader.Read())
        {
            if (user == null)
            {
                user = UserJWTResponse.FromUser(
                    reader.GetGuid(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    roles
                );
            }

            if (!reader.IsDBNull(3))
                roles.Add(reader.GetString(3));
        }

        dbConn.Close();
        return user;
    }

    public static bool CorrectPassword(SpotifyDBConnection dbConn, string email, string password)
    {
        dbConn.Open();
        string sql = "SELECT Password, Salt FROM Users WHERE Email = @Email";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Email", email);

        using SqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            dbConn.Close();
            return false;
        }

        string passwordHash = reader.GetString(0);
        string salt = reader.GetString(1);

        string computedPassword = Hash.ComputeHash(password, salt);

        dbConn.Close();

        return passwordHash == computedPassword;
    }
}