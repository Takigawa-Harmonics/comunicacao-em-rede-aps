using ComunicacaoEmRedesApi.Application.Extensions;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComunicacaoEmRedesApi.Api.Controllers;

[ApiController]
[Route("v1/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IResult> GetProfileById(Guid userId)
    {
        var profile = await _userService.GetProfileById(userId);
        return Results.Extensions.ToResultFormat(profile);
    }
}