using System.Net.Mail;
using ComunicacaoEmRedesApi.Application.Dtos;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;

namespace ComunicacaoEmRedesApi.Domain.Services;

public class SessionService : ISessionService
{
    private readonly IUserRepository _userRepository;

    public SessionService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task Register(RegisterRequestDto request)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.Password
        };

        var isValid = SessionDomainValidations.IsEmailDomainValid(user.Email);
        if (isValid)
        {
            Console.WriteLine("Nice one! User saved!");
            await _userRepository.SaveUserAsync(user);
            return;
        }
        
        Console.WriteLine("Oops! Invalid email!");
    }

    private static class SessionDomainValidations
    {
        public static bool IsEmailDomainValid(string email)
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
}