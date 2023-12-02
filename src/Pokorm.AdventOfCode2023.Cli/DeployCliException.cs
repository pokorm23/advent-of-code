namespace Pokorm.AdventOfCode2023.Cli;

public class DeployCliException : Exception
{
    public DeployCliException(string? message, Exception? innerException = null) : base(message, innerException) { }
}
