using ComunicacaoEmRedesApi.Application.Dtos.Session;
using ComunicacaoEmRedesApi.Application.Extensions;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ComunicacaoEmRedesApi.Api.Controllers;

[ApiController]
[Route("/v1/session")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public SessionController(ISessionService sessionService, ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _sessionService = sessionService;
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _sessionService.Register(request);
        return Results.Extensions.ToResultFormat(response);
    }

    [HttpPost("login")]
    [EnableRateLimiting(nameof(RateLimiterPolicies.SessionPolicy))]
    public async Task<IResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _sessionService.Login(request);
        Response.Cookies.Append(nameof(AvailableCookies.SessionToken), response.Value!.Token.Value, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = response.Value.Token.Expiration
        });
        Console.WriteLine(Request.Cookies[nameof(AvailableCookies.SessionToken)]);
        return Results.Extensions.ToResultFormat(response);
    }

    [HttpGet("me")]
    public async Task<IResult> Me()
    {
        var cookieValue = Request.Cookies[nameof(AvailableCookies.SessionToken)];

        if (string.IsNullOrEmpty(cookieValue))
            return Results.Unauthorized();

        var tokenOption = await _tokenRepository.GetTokenByValue(cookieValue);

        if (tokenOption.IsNone)
            return Results.Unauthorized();

        var token = tokenOption.First();

        if (token.IsRevoked || token.Expiration < DateTime.UtcNow)
            return Results.Unauthorized();

        var userOption = await _userRepository.GetProfileByIdAsync(token.UserId);

        if (userOption.IsNone)
            return Results.Unauthorized();

        var user = userOption.First();

        return Results.Ok(new { user.Id, user.Email });
    }

    [HttpPost("/logout")]
    public async Task Logout(Guid userId)
    {
        await _sessionService.Logout(userId);
        Response.Cookies.Delete(nameof(AvailableCookies.SessionToken));
    }
}