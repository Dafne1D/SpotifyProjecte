using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Utils;

namespace SpotifyAPI.Repository;

static class PermissionADO
{
    public static List<Permission> GetAll(SpotifyDBConnection dbConn)
    {
        List<Permission> permissions = new();

        dbConn.Open();
        string sql = "SELECT Id, Name, Description FROM Permissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            permissions.Add(new Permission
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            });
        }

        dbConn.Close();
        return permissions;
    }



}