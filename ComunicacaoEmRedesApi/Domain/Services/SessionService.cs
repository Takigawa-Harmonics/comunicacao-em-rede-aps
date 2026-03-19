using System.Text.RegularExpressions;
using ComunicacaoEmRedesApi.Application.Dtos;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Results;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using ComunicacaoEmRedesApi.Infrastructure.Security.Interfaces;

namespace ComunicacaoEmRedesApi.Domain.Services;

public class SessionService : ISessionService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncryption _encryption;

    public SessionService(IUserRepository userRepository, IPasswordEncryption encryption)
    {
        _userRepository = userRepository;
        _encryption = encryption;
    }
    
    public async Task<Result<RegisterResponseDto>> Register(RegisterRequestDto request)
    {
        var getErrors = SessionDomainValidations.GetRegisterErrors(request.Email, request.Password);

        if (getErrors.Count > 0)
        {
            return Result<RegisterResponseDto>.Failure(getErrors);
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.Password
        };

        user.PasswordHash = HashUserPassword(user);
        
        await _userRepository.SaveUserAsync(user);
        var response = RegisterResponseDto.Get(user.Email);

        return Result<RegisterResponseDto>.Success(response);
    }

    private string HashUserPassword(User user)
    {
        return _encryption.HashPassword(user, user.PasswordHash);
    }
    
    private static class SessionDomainValidations
    {
        public static List<string> GetRegisterErrors(string email, string password)
        {
            var errors = new List<string>();

            if (!IsEmailDomainValid(email))
            {
                errors.Add("Email domain is not valid!");
            }

            if (!IsPasswordStructureValid(password))
            {
                errors.Add("Password must contain letters, numbers and a special character!");
            }

            return errors;
        }
        
        private static bool IsEmailDomainValid(string email)
        {
            var providers = Enum.GetNames<AvailableEmailProviders>().Select(e => e.ToLower());
            return DoesEmailEndsWithDomain(email, providers);
        }

        private static bool IsPasswordStructureValid(string password)
        {
            return SessionDomainRegex.PasswordDefaultPattern.IsMatch(password);
        }

        private static bool DoesEmailEndsWithDomain(string email, IEnumerable<string> providers) =>
            providers.Any(provider => email.EndsWith(provider + AvailableDomains.DotCom) || email.EndsWith(provider + AvailableDomains.DotComBr));

        private struct AvailableDomains
        {
            public const string DotCom = ".com";
            public const string DotComBr = ".com.br";
        }
    }

    private static class SessionDomainRegex
    {
        private const int OneSecond = 1000;
        private static readonly TimeSpan Interval = new(OneSecond);
        
        public static readonly Regex PasswordDefaultPattern 
            = new(AvailableRegex.AllowLettersNumbersAndSomeSpecialChars, RegexOptions.Compiled, Interval);

        private struct AvailableRegex
        {
            public const string AllowLettersNumbersAndSomeSpecialChars = "^[a-zA-Z0-9 _.\\-@!?,:;()]+$";
        }
    }
}