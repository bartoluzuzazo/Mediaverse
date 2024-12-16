namespace MediaVerse.Tests.Tests.Entry;

[Collection("ClientHost")]
public class EntryTests : MediaVerseTestBase
{
    public EntryTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
    }

    

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/entry?searchTerm=%27t%27");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}