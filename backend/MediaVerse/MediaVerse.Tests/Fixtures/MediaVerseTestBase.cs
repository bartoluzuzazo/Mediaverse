using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Queries.UserQueries;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Tests;

public abstract class MediaVerseTestBase
    : IAsyncLifetime
{
    protected MediaVerseTestBase(ClientHostFixture clientHostFixture)
    {
        ClientHostFixture = clientHostFixture;
    }

    protected ClientHostFixture ClientHostFixture { get; }
    protected MediaVerseApplicationFactory<Program> Factory => ClientHostFixture.Factory;
    protected IServiceScope CreateScope() => Factory.Services.CreateScope();
    protected IServiceScope Scope;

    protected async Task<HttpClient> CreateAuthenticatedUserClientAsync() => await CreateUserAsync("user@admin.com");
    protected async Task<HttpClient> CreateAuthenticatedAdminClientAsync() => await CreateUserAsync("admin@admin.com");

    private async Task<HttpClient> CreateUserAsync(string email)
    {
        var client = Factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/user/login",
            new LoginUserQuery() { Email = email, Password = "admin" });

        var responseData = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData?.Token);
        return client;
    }

    public virtual async Task InitializeAsync()
    {
        Scope = CreateScope();
        
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Factory.DeleteAllDataAsync();
    }
}