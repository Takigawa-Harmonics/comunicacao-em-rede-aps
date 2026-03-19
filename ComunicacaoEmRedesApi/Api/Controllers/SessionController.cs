using ComunicacaoEmRedesApi.Application.Dtos;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _sessionService.Register(request);
        
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}