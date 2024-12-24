using System.Net;
using System.Net.Http.Json;
using MediaVerse.Client.Application.Queries.UserQueries;

namespace MediaVerse.Tests.Tests.User;

[Collection("ClientHost")]
public class GetUserTests : MediaVerseTestBase
{
    public GetUserTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task GetUser_Should_ReturnNotFoundForNonExistingUser()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/af7c1fe6-d669-4000-0000-e9733f0de7a8");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetUser_Should_ReturnSuccessForExistingUser()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/af7c1fe6-d669-414e-b066-e9733f0de7a8");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}