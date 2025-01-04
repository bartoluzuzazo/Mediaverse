using System.Net;
using System.Net.Http.Json;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.User;

[Collection("ClientHost")]
public class RegisterTests : MediaVerseTestBase
{
    public RegisterTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task RegisterUser_Should_CreateUserInDatabase()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/register", new RegisterUserCommand()
        {
            Username = "test",
            Email = "test@test.com",
            Password = "test",
        });

        // Assert
        response.EnsureSuccessStatusCode();
        var scope = Factory.Services.CreateAsyncScope();

        var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<Domain.Entities.User>>();
        var user = await userRepository.FirstOrDefaultAsync(new GetUserByEmailSpecification("test@test.com"));
        Assert.NotNull(user);
    }

    [Fact]
    public async Task RegisterUser_Should_ReturnConflictWhenUserWithSameEmailExists()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/register", new RegisterUserCommand()
        {
            Username = "test",
            Email = "admin@admin.com",
            Password = "test",
        });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
    
    [Fact]
    public async Task RegisterUser_Should_ReturnConflictWhenUserWithSameUsernameExists()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/user/register", new RegisterUserCommand()
        {
            Username = "admin",
            Email = "admin2@admin.com",
            Password = "test",
        });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}