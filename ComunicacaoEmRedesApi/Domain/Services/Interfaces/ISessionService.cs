using ComunicacaoEmRedesApi.Application.Dtos;
using ComunicacaoEmRedesApi.Domain.Results;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface ISessionService
{
    Task<Result<RegisterResponseDto>> Register(RegisterRequestDto request);
}