using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pokorm.AdventOfCode2023;

public static class Extensions
{
    public static IServiceCollection AddAdventOfCode(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.TryAddSingleton<IInputService, InputService>();

        services.TryAddSingleton<IDayFactory, DayFactory>();

        services.Scan(scan => scan
                              .FromAssemblyOf<IDay>()
                              .AddClasses(classes => classes.AssignableTo<IDay>())
                              .AsImplementedInterfaces()
                              .WithSingletonLifetime());

        return services;
    }
}
