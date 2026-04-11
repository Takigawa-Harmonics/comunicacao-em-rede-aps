using ComunicacaoEmRedesApi.Application.Dtos.Session;
using ComunicacaoEmRedesApi.Application.Extensions;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;  

namespace ComunicacaoEmRedesApi.Api.Controllers;

[ApiController]
[Route("/v1/session")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
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

    [HttpPost("/logout")]
    public async Task Logout(Guid userId)
    {
        await _sessionService.Logout(userId);
        Response.Cookies.Delete(nameof(AvailableCookies.SessionToken));
    }
}