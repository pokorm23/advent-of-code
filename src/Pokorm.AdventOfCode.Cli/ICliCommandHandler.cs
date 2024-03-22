namespace Pokorm.AdventOfCode.Cli;

public interface ICliCommandHandler<in T> where T : class
{
    Task HandleAsync(T command, CancellationToken cancellationToken);
}
