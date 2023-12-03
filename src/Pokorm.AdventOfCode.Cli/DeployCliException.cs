namespace Pokorm.AdventOfCode.Cli;

public class DeployCliException : Exception
{
    public DeployCliException(string? message, Exception? innerException = null) : base(message, innerException) { }
}
