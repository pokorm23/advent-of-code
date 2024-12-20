namespace Pokorm.AdventOfCode.Tests.Logging;

public record XUnitFormattingState(bool SuppressColor = false)
{
    public static XUnitFormattingState NoColor = new XUnitFormattingState(true);
}
