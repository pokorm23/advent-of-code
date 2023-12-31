﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pokorm.AdventOfCode.Cli;

public static class Extensions
{
    public static IServiceCollection AddAdventOfCodeApp(this IServiceCollection services)
    {
        services.AddAdventOfCode();

        services.TryAddTransient<RunCommandHandler>();

        return services;
    }
}
