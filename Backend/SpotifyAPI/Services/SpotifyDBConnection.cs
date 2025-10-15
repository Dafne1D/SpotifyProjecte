using Microsoft.Data.SqlClient;

namespace SpotifyAPI.Services
{
    public class SpotifyDBConnection
    {
        private readonly string _connectionString;
        public SqlConnection? sqlConnection;
        public SpotifyDBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Open()
        {
            sqlConnection = new SqlConnection(_connectionString);

            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Close()
        {
            sqlConnection!.Close();
        }
    }
}
