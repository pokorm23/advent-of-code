using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pokorm.AdventOfCode2023.Cli;

public static class Extensions
{
    public static IServiceCollection AddAdventOfCodeApp(this IServiceCollection services)
    {
        services.AddAdventOfCode();

        services.TryAddTransient<RunCommandHandler>();

        return services;
    }
}
