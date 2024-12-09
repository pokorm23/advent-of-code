using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Tests.Logging;

public class XUnitLoggerOptions
{
    public Func<string?, LogLevel, bool> Filter { get; set; } = static (c, l) => true;
}
