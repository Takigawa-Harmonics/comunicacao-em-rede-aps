using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Services;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using ComunicacaoEmRedesApi.Infrastructure.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Security;
using ComunicacaoEmRedesApi.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();