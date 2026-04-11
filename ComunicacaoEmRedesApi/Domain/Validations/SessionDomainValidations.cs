using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Results;

namespace ComunicacaoEmRedesApi.Domain.Validations;

public static class SessionDomainValidations
{
    public static List<Error> GetRegisterErrors(string email, string password)
    {
        var errors = new List<Error>();
            
        if (!IsEmailDomainValid(email)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidEmail, "Email domain is not valid!");
        if (!PasswordValidations.IsPasswordLengthCorrect(password)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidPassword, "Password length must be between 8 and 15 characters!");
        if (!PasswordValidations.IsPasswordStructureValid(password)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidPassword, "Password must contain letters, numbers and a special character!");

        return errors.Select(a => a)
            .OrderBy(a => a.Message)
            .ToList();
    }
        
    private static bool IsEmailDomainValid(string email)
    {
        var providers = Enum.GetNames<AvailableEmailProviders>().Select(e => e.ToLower());
        return DoesEmailEndsWithDomain(email, providers);
    }

    private static bool DoesEmailEndsWithDomain(string email, IEnumerable<string> providers) 
        => providers.Any(provider => email.EndsWith(provider + AvailableDomains.DotCom) || email.EndsWith(provider + AvailableDomains.DotComBr));
        
    private struct AvailableDomains 
    {
        public const string DotCom = ".com"; 
        public const string DotComBr = ".com.br";
    }
}