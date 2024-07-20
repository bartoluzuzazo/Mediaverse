using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediaVerse.Client.Application.Extensions.MediatR;

public static class MediatRExtensions
{
    public static IServiceCollection RegisterMediatR(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        return serviceCollection;
    }
}