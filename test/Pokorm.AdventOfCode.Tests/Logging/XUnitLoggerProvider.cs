using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Tests.Logging;

[ProviderAlias("XUnit")]
public class XUnitLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, XUnitLogger> loggers = [ ];

    private readonly XUnitLoggerOptions options;

    private readonly ITestOutputHelperAccessor outputHelperAccessor;

    public XUnitLoggerProvider(ITestOutputHelper outputHelper, XUnitLoggerOptions options)
        : this(new TestOutputHelperAccessor(outputHelper), options) { }

    public XUnitLoggerProvider(ITestOutputHelperAccessor accessor, XUnitLoggerOptions options)
    {
        this.outputHelperAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public virtual ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(categoryName, (name) => new (name, this.outputHelperAccessor, this.options));
    }

    public void Dispose() { }
}
