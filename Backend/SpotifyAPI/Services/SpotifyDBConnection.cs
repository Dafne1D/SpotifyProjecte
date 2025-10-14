using Microsoft.Data.SqlClient;

namespace SpotifyAPI.Services
{
    public class SpotifyDBConnection
    {
        private readonly string _connectionString;
        public SpotifyDBConnection(string connectionString) { _connectionString = connectionString; }
        public SqlConnection Create() => new SqlConnection(_connectionString);
    }
}
