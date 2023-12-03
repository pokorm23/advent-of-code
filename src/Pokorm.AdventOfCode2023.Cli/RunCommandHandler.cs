using System.CommandLine;
using System.Security.Cryptography;

namespace Pokorm.AdventOfCode2023.Cli;

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
        var day = this.dayFactory.GetDay(command.Day);

        int? result;

        if (command.Bonus)
        {
            result = await day.SolveBonusAsync();
        }
        else
        {
            result = await day.SolveAsync();
        }

        this.console.WriteLine($"Result for day {command.Day}{(command.Bonus ? " (bonus)" : "")}:");

        this.console.WriteLine(result?.ToString() ?? "");
    }
}
