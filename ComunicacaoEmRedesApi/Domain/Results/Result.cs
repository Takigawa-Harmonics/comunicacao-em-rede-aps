using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Domain.Results;

// todo: add error object, with message and error type
public class Result<T> 
{
    public bool IsSuccess { get; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Value { get; }
    
    public List<string> Errors { get; }
    
    protected Result(bool isSuccess, T? value, List<string> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T? value) => new(true, value, []);
    public static Result<T> Failure(List<string> errors) => new(false, default, errors);
}