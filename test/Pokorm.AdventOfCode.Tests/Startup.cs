using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pokorm.AdventOfCode.Tests.Logging;
using Xunit.DependencyInjection;

namespace Pokorm.AdventOfCode.Tests;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddXUnitLogging();
        });
    }
}