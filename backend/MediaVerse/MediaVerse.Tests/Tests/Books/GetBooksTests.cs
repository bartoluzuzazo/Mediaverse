namespace MediaVerse.Tests.Tests.Books;

[Collection("ClientHost")]
public class GetBooksTests : MediaVerseTestBase
{
    public GetBooksTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task GetBooks_Should_ReturnSuccess()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/book/page?page=1&size=10&Order=0&direction=0");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}