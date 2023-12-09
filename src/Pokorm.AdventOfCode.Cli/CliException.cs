namespace Pokorm.AdventOfCode.Cli;

public class CliException : Exception
{
    public CliException(string? message, Exception? innerException = null) : base(message, innerException) { }
}
