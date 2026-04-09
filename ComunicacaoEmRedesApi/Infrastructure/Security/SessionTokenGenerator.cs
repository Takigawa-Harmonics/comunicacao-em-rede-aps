using System.Security.Cryptography;

namespace ComunicacaoEmRedesApi.Infrastructure.Security;

public static class SessionTokenGenerator
{
    public static string Generate(int length = 50)
    {
        var bytes = new byte[length];
        using(var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);   
        }
    }
}