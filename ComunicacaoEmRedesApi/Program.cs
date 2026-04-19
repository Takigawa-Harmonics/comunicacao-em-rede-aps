using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Services;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using ComunicacaoEmRedesApi.Infrastructure.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Security;
using ComunicacaoEmRedesApi.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ComunicacaoEmRedesApi.Domain.Enums;
using ComunicacaoEmRedesApi.Domain.Results;
using ComunicacaoEmRedesApi.Infrastructure.Socket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordEncryption, PasswordEncryption>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=" + Path.Combine(builder.Environment.ContentRootPath, "app.db")));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(nameof(RateLimiterPolicies.SessionPolicy), opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(40);
        opt.AutoReplenishment = true;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        var timeoutResponse = Error.Get(Error.Codes.RequestLimitAchieved, Error.Messages.RequestLimitAchievedMessage);
        await context.HttpContext.Response.WriteAsJsonAsync(Result<string>.Failure(ErrorType.TooManyRequests, [timeoutResponse]), cancellationToken: token);
    };
});

builder.Services.AddHostedService<ChatSocketServer>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();