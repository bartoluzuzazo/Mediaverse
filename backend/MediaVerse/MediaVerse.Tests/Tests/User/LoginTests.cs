using System.Net;
using System.Net.Http.Json;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Client.Application.Queries.UserQueries;

namespace MediaVerse.Tests.Tests.User;

[Collection("ClientHost")]
public class LoginTests : MediaVerseTestBase
{
    public LoginTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task Login_Should_ReturnNotFoundForNonExistingUser()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/login", new LoginUserQuery()
        {
            Email = "non-exisiting@admin.com",
            Password = "admin",
        });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Login_Should_ReturnNotFoundForWrongPassword()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/login", new LoginUserQuery()
        {
            Email = "admin@admin.com",
            Password = "wrong-password",
        });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Login_Should_ReturnSuccessForCorrectCredentials()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/login", new LoginUserQuery()
        {
            Email = "admin@admin.com",
            Password = "admin",
        });

        // Assert
        response.EnsureSuccessStatusCode();
    }
}