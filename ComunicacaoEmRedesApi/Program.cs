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
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordEncryption, PasswordEncryption>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=" + Path.Combine(builder.Environment.ContentRootPath, "app.db")));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
    options.RejectionStatusCode = 429;
});

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