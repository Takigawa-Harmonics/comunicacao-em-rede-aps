using ComunicacaoEmRedesApi.Application.Dtos.Profile;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Results;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;

namespace ComunicacaoEmRedesApi.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<ProfileDto>> GetProfileById(Guid userId)
    {
        var searchForUser = await _userRepository.GetProfileByIdAsync(userId);
        var user = searchForUser.First();

        var response = ProfileDto.Get(user);

        return Result<ProfileDto>.Success(response);
    }
}