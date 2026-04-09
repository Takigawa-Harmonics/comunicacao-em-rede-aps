using ComunicacaoEmRedesApi.Application.Dtos;
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
        return Results.Extensions.ToResultFormat(response);
    }
}