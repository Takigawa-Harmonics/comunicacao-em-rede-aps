using ComunicacaoEmRedesApi.Infrastructure.Security;
using Xunit.Abstractions;

namespace Tests.Unit;

public class SessionTokenGeneratorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SessionTokenGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Should_GenerateToken_Then_Return_It()
    {
        var token = SessionTokenGenerator.Generate();

        _testOutputHelper.WriteLine(token);
        
        Assert.NotNull(token);
        Assert.Equal(68, token.Length);
    }
}