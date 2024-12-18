using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pokorm.AdventOfCode.Tests.Logging;

namespace Pokorm.AdventOfCode.Tests;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);

            builder.Services.TryAddSingleton<TestOutputHelperAccessorWrapper>();

            builder.Services.AddSingleton<ILoggerProvider>(provider => new XUnitLoggerProvider(
                provider.GetRequiredService<TestOutputHelperAccessorWrapper>(),
                provider.GetRequiredService<IOptions<XUnitLoggerOptions>>().Value));
        });
    }
}