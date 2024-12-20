using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;

namespace Pokorm.AdventOfCode.Tests.Logging;

[ProviderAlias("XUnit")]
public sealed class XUnitLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, XUnitLogger> loggers = [ ];

    private readonly XUnitLoggerOptions options;

    private readonly ITestOutputHelperAccessor outputHelperAccessor;

    public XUnitLoggerProvider(ITestOutputHelperAccessor accessor, XUnitLoggerOptions options)
    {
        this.outputHelperAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(categoryName, static (name, s) => new (name, s.outputHelperAccessor, s.options), (this.outputHelperAccessor, this.options));
    }

    public void Dispose() {}
}
