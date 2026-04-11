using ComunicacaoEmRedesApi.Application.Dtos.Session;
using ComunicacaoEmRedesApi.Domain.Results;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface ISessionService
{
    Task<Result<RegisterResponseDto>> Register(RegisterRequestDto request);
    Task<Result<LoginResponseDto>> Login(LoginRequestDto request);
    Task Logout(Guid userId);
}