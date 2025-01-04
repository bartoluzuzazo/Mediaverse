using System.Net;

namespace MediaVerse.Tests.Tests.User;

[Collection("ClientHost")]
public class GetUserByEmail : MediaVerseTestBase
{
    public GetUserByEmail(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }
    
    [Fact]
    public async Task GetUser_Should_ReturnNotFoundForNonExistingUser()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/email/non-existing");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetUser_Should_ReturnSuccessForExistingUser()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/email/admin@admin.com");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}