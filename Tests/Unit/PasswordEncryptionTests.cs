using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;

namespace Tests.Unit;

public class PasswordEncryptionTests
{
    private readonly User _subjectUser = new()
    {
        Email = "subject@email.com",
        PasswordHash = "subject_password"
    };

    private readonly PasswordEncryption _encryption;
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public PasswordEncryptionTests()
    {
        _encryption = new PasswordEncryption(_passwordHasher);
    }

    [Fact]
    public void Should_HashPassword_When_Called()
    {
        var hash = _encryption.HashPassword(_subjectUser, _subjectUser.PasswordHash);

        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual(_subjectUser.PasswordHash, hash);
    }

    [Fact]
    public void Should_Return_True_When_Passwords_Match()
    {
        var hash = _encryption.HashPassword(_subjectUser, _subjectUser.PasswordHash);
        var verify = _encryption.VerifyPassword(_subjectUser, hash, _subjectUser.PasswordHash);
        
        Assert.True(verify);
    }

    [Fact]
    public void Should_Return_False_When_Passwords_DoNot_Match()
    {
        var hash = _encryption.HashPassword(_subjectUser, "wrong_password");
        var verify = _encryption.VerifyPassword(_subjectUser, hash, _subjectUser.PasswordHash);
        
        Assert.False(verify);
    }
}