using Ardalis.Specification;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.Books;

[Collection("ClientHost")]
public class GetBookTests : MediaVerseTestBase
{
    public GetBookTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task GetBook_Should_ReturnSuccess()
    {
        // Arrange
        var client = Factory.CreateClient();
        await base.InitializeAsync();

        var bookRepository = Scope.ServiceProvider.GetService<IRepository<Domain.Entities.Book>>();
        var book = await bookRepository?.FirstOrDefaultAsync(new SingleResultSpecification<Domain.Entities.Book>())!;
        
        // Act
        var response = await client.GetAsync($"/api/book/{book?.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}