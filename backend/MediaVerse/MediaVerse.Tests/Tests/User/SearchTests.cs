namespace MediaVerse.Tests.Tests.User;

[Collection("ClientHost")]
public class SearchTests : MediaVerseTestBase
{
    public SearchTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task Search_Should_ReturnSuccess()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/search?query=admin&page=1&size=10");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}