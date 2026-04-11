using ComunicacaoEmRedesApi.Application.Dtos.Session;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Results;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using ComunicacaoEmRedesApi.Domain.Validations;
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
        
        var user = new User { Email = request.Email, PasswordHash = request.Password };

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
}