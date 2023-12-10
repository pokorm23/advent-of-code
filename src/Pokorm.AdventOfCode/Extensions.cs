using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pokorm.AdventOfCode;

public static class Extensions
{
    public static IServiceCollection AddAdventOfCode(this IServiceCollection services)
    {
        services.TryAddSingleton<IInputService, InputService>();

        services.TryAddSingleton<IDayFactory, DayFactory>();

        services.Scan(scan => scan
                              .FromAssemblyOf<IDay>()
                              .AddClasses(classes => classes.AssignableTo<IDay>())
                              .AsImplementedInterfaces()
                              .WithSingletonLifetime());

        return services;
    }

    public static string[] FullSplit(this string value, params char[] split) => value.Split(split, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
