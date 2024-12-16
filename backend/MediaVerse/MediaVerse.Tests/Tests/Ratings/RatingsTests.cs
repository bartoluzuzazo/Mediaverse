using System.Net;
using Ardalis.Specification;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests.Tests.Ratings;

[Collection("ClientHost")]
public class RatingsTests : MediaVerseTestBase
{
    private Domain.Entities.Entry? entry;

    public RatingsTests(ClientHostFixture clientHostFixture) : base(clientHostFixture)
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
            Id = Guid.NewGuid(),
        })!;

        await ratingsRepository?.SaveChangesAsync()!;
    }

    [Fact]
    public async Task ForAuthenticatedUser_ShouldReturnRatings()
    {
        // Arrange
        var client = await CreateAuthenticatedUserClientAsync();

        // Act
        var response = await client.GetAsync($"api/entries/{entry.Id}/ratings/users-rating");

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task ForAuthenticatedUserWithoutRating_ShouldReturnNotFound()
    {
        // Arrange
        var client = await CreateAuthenticatedAdminClientAsync();

        // Act
        var response = await client.GetAsync($"api/entries/{entry.Id}/ratings/users-rating");

        // Assert
       Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
    }
}