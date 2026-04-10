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
    private readonly ITokenService _tokenService;
    private readonly IPasswordEncryption _encryption;

    public SessionService(IUserRepository userRepository, ITokenService tokenService, IPasswordEncryption encryption)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _encryption = encryption;
    }
    
    public async Task<Result<RegisterResponseDto>> Register(RegisterRequestDto request)
    {
        var registerDomainErrors = SessionDomainValidations.GetRegisterErrors(request.Email, request.Password);

        if (registerDomainErrors.Count > 0)
        {
            return Result<RegisterResponseDto>.Failure(ErrorType.BadRequest, registerDomainErrors);
        }

        if (await DoesRequestedEmailAlreadyExists(request.Email))
        {
            var conflictError = Error.Get(Error.Codes.EmailAlreadyExists, "This email already exists!");
            return Result<RegisterResponseDto>.Failure(ErrorType.Conflict, [conflictError]);
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

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto request)
    {
        var lookForUser = await _userRepository.GetUserByEmailAsync(request.Email);
        
        if (lookForUser.IsNone)
        {
            var error = Error.Get(Error.Codes.UserNotFound, Error.Messages.InvalidLoginMessage);
            return Result<LoginResponseDto>.Failure(ErrorType.NotFound, [error]);
        }

        var user = lookForUser.First();

        if (!DoesRequestPasswordMatchesWithExistent(user, request.Password))
        {
            var error = Error.Get(Error.Codes.InvalidPassword, Error.Messages.InvalidLoginMessage);
            return Result<LoginResponseDto>.Failure(ErrorType.BadRequest, [error]);
        }

        var token = await _tokenService.ManageTokenCreationFlow(user.Id);
        
        var response = LoginResponseDto.Get($"Welcome, {user.Email.ToUpper()}", token, DateTime.UtcNow);
        return Result<LoginResponseDto>.Success(response);
    }

    public async Task Logout(Guid userId)
    {
        await _tokenService.SetTokenAsRevoked(userId);
    }

    private async Task<bool> DoesRequestedEmailAlreadyExists(string email)
    {
        return await _userRepository.DoesEmailExists(email);
    }
    
    private string HashUserPassword(User user)
    {
        return _encryption.HashPassword(user, user.PasswordHash);
    }

    private bool DoesRequestPasswordMatchesWithExistent(User user, string requestPassword)
    {
        if (string.IsNullOrEmpty(requestPassword)) return false;
        return _encryption.VerifyPassword(user, user.PasswordHash, requestPassword);
    }
    
    private static class SessionDomainValidations
    {
        public static List<Error> GetRegisterErrors(string email, string password)
        {
            var errors = new List<Error>();
            
            if (!IsEmailDomainValid(email)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidEmail, "Email domain is not valid!");
            if (!IsPasswordLengthCorrect(password)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidPassword, "Password length must be between 8 and 15 characters!");
            if (!IsPasswordStructureValid(password)) Error.AddErrorToTargetList(errors, Error.Codes.InvalidPassword, "Password must contain letters, numbers and a special character!");

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

        private static bool IsPasswordLengthCorrect(string password) 
            => password.Length is >= 8 and <= 15;
        
        private static bool IsPasswordStructureValid(string password)
        {
            bool hasLetter = false, hasNumber = false, hasSpecialChar = false;
            const string allowedSpecialChars = "._-@!?,:;()";
            
            foreach (var digit in password)
            {
                if (char.IsLetter(digit)) hasLetter = true;
                else if (char.IsNumber(digit)) hasNumber = true;
                else if (allowedSpecialChars.Contains(digit)) hasSpecialChar = true;
                else return false;
            }
            
            return hasLetter && hasNumber && hasSpecialChar;
        }
        
        private struct AvailableDomains
        {
            public const string DotCom = ".com";
            public const string DotComBr = ".com.br";
        }
    }
}