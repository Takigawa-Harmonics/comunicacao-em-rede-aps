using ComunicacaoEmRedesApi.Application.Dtos.Profile;
using ComunicacaoEmRedesApi.Domain.Results;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface IUserService
{
    Task<Result<ProfileDto>> GetProfileById(Guid userId);
}