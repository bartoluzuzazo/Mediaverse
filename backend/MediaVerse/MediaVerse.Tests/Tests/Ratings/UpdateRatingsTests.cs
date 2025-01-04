using System.Net;
using System.Net.Http.Json;
using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.RatingDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.Ratings;

[Collection("ClientHost")]
public class UpdateRatingsTests : MediaVerseTestBase
{
    private Domain.Entities.Entry? entry;
    private readonly Guid _ratingId = Guid.NewGuid();


    public UpdateRatingsTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var entryRepository = Scope.ServiceProvider.GetService<IRepository<Domain.Entities.Entry>>();
        var userRepository = Scope.ServiceProvider.GetService<IRepository<Domain.Entities.User>>();
        var ratingsRepository = Scope.ServiceProvider.GetService<IRepository<Rating>>();
        entry = await entryRepository?.FirstOrDefaultAsync(new SingleResultSpecification<Domain.Entities.Entry>())!;
        var user = await userRepository?.FirstOrDefaultAsync(new GetUserByEmailSpecification("user@admin.com"))!;

        await ratingsRepository?.AddAsync(new Rating()
        {
            Entry = entry!,
            Grade = 5,
            User = user!,
            Modifiedat = DateTime.Now,
            Id = _ratingId,
        })!;

        await ratingsRepository?.SaveChangesAsync()!;
    }

    [Fact]
    public async Task UpdateRating_Should_ReturnSuccessForExisitingRating()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PutAsJsonAsync($"api/ratings/{_ratingId}",
            new PutRatingDto { Grade = 5, EntryId = entry!.Id });

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task UpdateRating_Should_ReturnNotFoundForNonExisitingRating()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PutAsJsonAsync($"api/ratings/a890a108-0f93-40b1-8d57-88e611c6e8e4",
            new PutRatingDto { Grade = 5, EntryId = entry!.Id });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    
    [Fact]
    public async Task UpdateRating_Should_ReturnNotFoundForNonExisitingEntry()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.PutAsJsonAsync($"api/ratings/{_ratingId}",
            new PutRatingDto { Grade = 5, EntryId = Guid.Parse("a890a108-0f93-40b1-8d57-88e611c6e8e4") });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
}