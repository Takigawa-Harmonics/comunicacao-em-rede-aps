using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Domain.Results;

public class Error
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }
    
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    public static Error Get(string code, string message)
    {
        return new Error
        {
            Code = code,
            Message = message
        };
    }
    
    public static void AddErrorToTargetList(List<Error> target, string code, string message)
        => target.Add(new Error{ Code = code, Message = message });
    
    public struct Codes
    {
        public const string InvalidEmail = "INVALID_EMAIL";
        public const string EmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
        public const string InvalidPassword = "INVALID_PASSWORD";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string RequestLimitAchieved = "TOO_MANY_REQUESTS";
    }

    public struct Messages
    {
        public const string InvalidLoginMessage = "Invalid email or password!";
        public const string RequestLimitAchievedMessage = "Something went wrong. Please, try again later!";
    }
}