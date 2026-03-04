namespace SpotifyAPI.Domain.Entities;

public class User
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Salt { get; set; } = "";
}