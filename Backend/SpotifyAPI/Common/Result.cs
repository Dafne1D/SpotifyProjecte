namespace SpotifyAPI.Common;

public class Result
{
    public bool IsOk { get; }
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }

    private Result(bool ok, string? message = null, string? code = null)
    {
        IsOk = ok;
        ErrorCode = code;
        ErrorMessage = message;
    }

    public static Result Ok() => new Result(true);
    public static Result Failure(string message, string? code) =>
        new Result(false, message, code); 
}
