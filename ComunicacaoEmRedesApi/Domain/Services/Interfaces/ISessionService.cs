using ComunicacaoEmRedesApi.Application.Dtos;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface ISessionService
{
    Task Register(RegisterRequestDto request);
}