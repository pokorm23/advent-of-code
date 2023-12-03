using System.CommandLine;

namespace Pokorm.AdventOfCode.Cli;

public class RunCommandHandler
{
    private readonly IConsole console;
    private readonly IDayFactory dayFactory;

    public RunCommandHandler(IConsole console,
        IDayFactory dayFactory)
    {
        this.console = console;
        this.dayFactory = dayFactory;
    }

    public Task HandleAsync(RunCliCommand command, CancellationToken cancellationToken)
    {
        var day = this.dayFactory.GetDay(command.Year, command.Day);

        var result = command.Bonus ? day.SolveBonusAsync() : day.SolveAsync();

        this.console.WriteLine($"Result for {command.Year} day {command.Day}{(command.Bonus ? " (bonus)" : "")}:");

        this.console.WriteLine(result.ToString());

        return Task.CompletedTask;
    }
}
