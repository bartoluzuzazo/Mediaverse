namespace MediaVerse.Tests.Tests.Entry;

[Collection("ClientHost")]
public class EntryTests : MediaVerseTestBase
{
    public EntryTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task GetEntries_Should_ReturnSuccess()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/entry?searchTerm=%27t%27");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}