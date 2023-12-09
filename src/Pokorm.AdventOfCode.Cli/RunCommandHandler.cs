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

    public async Task HandleAsync(RunCliCommand command, CancellationToken cancellationToken)
    {
        var day = this.dayFactory.GetDay(command.Year, command.Day);

        var cancelTask = Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested) { }
        }, cancellationToken);

        var workTask = Task.Run(() => command.Bonus ? day.SolveBonus() : day.Solve(), cancellationToken);

        var rt = await Task.WhenAny(new[]
        {
            workTask,
            cancelTask
        });

        if (rt == cancelTask)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        var result = await workTask;

        this.console.WriteLine($"Result for {command.Year} day {command.Day}{(command.Bonus ? " (bonus)" : "")}:");

        this.console.WriteLine(result.ToString());
    }
}
