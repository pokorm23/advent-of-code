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
                              .AddClasses(classes => classes.Where(t => t.Name.StartsWith("Day")))
                              .AsSelf()
                              .WithSingletonLifetime());

        return services;
    }

    public static string[] FullSplit(this string value, params char[] split) => value.Split(split, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static string[] FullSplit(this string value, string split) => value.Split(split, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static (long, long) Nums2(this string[] value) => (long.Parse(value[0]), long.Parse(value[0]));
}
