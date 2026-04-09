namespace ComunicacaoEmRedesApi.Domain.Enums;

public enum ErrorType
{
    Conflict = 409,
    NotFound = 404,
    BadRequest = 400,
    TooManyRequests = 429,
    NoError = 0
}