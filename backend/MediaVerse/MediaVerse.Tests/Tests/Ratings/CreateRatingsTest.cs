using System.Net;
using System.Net.Http.Json;
using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.RatingDTOs;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.Ratings;

[Collection("ClientHost")]
public class CreateRatingsTest : MediaVerseTestBase
{
    public CreateRatingsTest(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    private Domain.Entities.Entry? entry;
    

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var entryRepository = Scope.ServiceProvider.GetService<IRepository<Domain.Entities.Entry>>();
        entry = await entryRepository?.FirstOrDefaultAsync(new SingleResultSpecification<Domain.Entities.Entry>())!;
        
    }
    
    [Fact]
    public async Task CreateRating_Should_ReturnSuccessAndAddRatingForAuthenticatedUser()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PostAsJsonAsync($"api/entries/{entry.Id}/ratings", new PostRatingDto {Grade = 5});

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task CreateRating_Should_ReturnNotFoundForIncorrectEntryId()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PostAsJsonAsync($"api/entries/a890a108-0f93-40b1-8d57-88e611c6e8e4/ratings", new PostRatingDto {Grade = 5});

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}