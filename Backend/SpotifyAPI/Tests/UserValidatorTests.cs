using Xunit;
using SpotifyAPI.Validators;
using SpotifyAPI.Common;
using SpotifyAPI.DTO;

public class UserValidatorTests
{
    private static UserRequest CreateUserRequest(string username = "validuser", string email = "user@gmail.com", string password = "Password1")
    {
        return new UserRequest(username, email, password);
    }

    [Fact]
    public void Validate_Fails_When_Username_Is_Null()
    {
        UserRequest user = CreateUserRequest(username: "");

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("DADA_OBLIGATORIA", result.ErrorCode);
    }

    [Fact]
    public void Validate_Fails_When_Username_Is_Too_Long()
    {
        UserRequest user = CreateUserRequest(username: new string('a', 51));

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("LONGITUD_INCORRECTE", result.ErrorCode);
    }

    [Fact]
    public void Validate_Fails_When_Email_Is_Empty()
    {
        UserRequest user = CreateUserRequest(email: "");

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("EMAIL_OBLIGATORI", result.ErrorCode);
    }

    [Fact]
    public void Validate_Fails_When_Email_Is_Not_Gmail()
    {
        UserRequest user = CreateUserRequest(email: "user@yahoo.com");

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("EMAIL_INVALID", result.ErrorCode);
    }

    [Fact]
    public void Validate_Fails_When_Password_Is_Too_Short()
    {
        UserRequest user = CreateUserRequest(password: "Aa1");

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("PASSWORD_CURTA", result.ErrorCode);
    }

    [Fact]
    public void Validate_Fails_When_Password_Is_Weak()
    {
        UserRequest user = CreateUserRequest(password: "password"); // no upper, no digit

        Result result = UserValidator.Validate(user);

        Assert.False(result.IsOk);
        Assert.Equal("PASSWORD_DÈBIL", result.ErrorCode);
    }

    [Fact]
    public void Validate_Returns_Ok_When_User_Is_Valid()
    {
        UserRequest user = CreateUserRequest();

        Result result = UserValidator.Validate(user);

        Assert.True(result.IsOk);
    }
}
