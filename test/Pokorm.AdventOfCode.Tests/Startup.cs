using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokorm.AdventOfCode.Tests.Logging;
using ITestOutputHelperAccessor = Xunit.DependencyInjection.ITestOutputHelperAccessor;

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

internal sealed class TestOutputHelperAccessorWrapper(ITestOutputHelperAccessor accessor)
    : Logging.ITestOutputHelperAccessor
{
    ITestOutputHelper? Logging.ITestOutputHelperAccessor.OutputHelper
    {
        get => accessor.Output;
        set => accessor.Output = value;
    }
}
