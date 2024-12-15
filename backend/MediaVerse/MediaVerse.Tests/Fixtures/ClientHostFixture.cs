using Microsoft.AspNetCore.Mvc.Testing;

namespace MediaVerse.Tests;

public class ClientHostFixture : IDisposable
{
    public MediaVerseApplicationFactory<Program> Factory { get; }

    public ClientHostFixture()
    {
        Factory = new MediaVerseApplicationFactory<Program>();
    }

    public void Dispose()
    {
        Factory?.Dispose();
    }
}

[CollectionDefinition("ClientHost")]
public class ClientHostFixtureCollection : ICollectionFixture<ClientHostFixture> {}