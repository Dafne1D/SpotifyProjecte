using SpotifyAPI.Model;

namespace SpotifyAPI.Repository;

static class RoleADO
{
    public static List<Role> GetAll(SpotifyDBConnection dbConn)
    {
        List<Role> roles = new();

        dbConn.Open();
        string sql = "SELECT Id, Name, Description FROM Roles";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            roles.Add(new Role
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            });
        }

        dbConn.Close();
        return roles;
    }
}