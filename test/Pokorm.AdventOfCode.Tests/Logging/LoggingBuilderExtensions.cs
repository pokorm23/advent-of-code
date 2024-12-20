using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.DependencyInjection;

namespace Pokorm.AdventOfCode.Tests.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddXUnitLogging(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider>(provider => new XUnitLoggerProvider(
            provider.GetRequiredService<ITestOutputHelperAccessor>(),
            provider.GetRequiredService<IOptions<XUnitLoggerOptions>>().Value));

        return builder;
    }
}
