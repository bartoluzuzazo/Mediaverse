using System.Net;
using System.Net.Http.Json;
using Ardalis.Specification;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.Books;

[Collection("ClientHost")]
public class AddBookTests : MediaVerseTestBase
{
    public AddBookTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    [Fact]
    public async Task AddBook_Should_ReturnForbiddednForNonAdminUser()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PostAsJsonAsync("/api/book", new AddBookCommand(new AddEntryCommand
            (
                "Test",
                "test",
                DateTime.Today, "", []
            ), "test", "", [])
        );

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task AddBook_Should_ReturnSuccessForAdminUser()
    {
        // Arrange
        var client = await CreateAuthenticatedAdminClientAsync();

        // Act
        var response = await client.PostAsJsonAsync("/api/book", new AddBookCommand(new AddEntryCommand
            (
                "Test",
                "test",
                DateTime.Today, "", []
            ), "test", "", [])
        );

        // Assert
        response.EnsureSuccessStatusCode();

        var bookRepository = Scope.ServiceProvider.GetService<IRepository<Domain.Entities.Book>>();

        var book = await bookRepository?.FirstOrDefaultAsync(new GetBookByIsbnSpecification("test"))!;
        Assert.NotNull(book);
    }
}