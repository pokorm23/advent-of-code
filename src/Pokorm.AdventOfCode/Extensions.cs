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

    public static (long, long) Nums2(this string[] value) => (long.Parse(value[0]), long.Parse(value[1]));

    public static (string[], string[]) StrParts2(this string[] value) => string.Join(Environment.NewLine, value)
                                                                           .ReplaceLineEndings()
                                                                           .FullSplit(Environment.NewLine + Environment.NewLine)
                                                                           .StrLines2();
    public static (string, string) Str2(this string[] value) => (value[0], (value[1]));

    public static string[] Lines(this string value) => value.ReplaceLineEndings().FullSplit(Environment.NewLine);

    public static (string[], string[]) StrLines2(this string[] value) => (value[0].Lines(), value[1].Lines());
}
