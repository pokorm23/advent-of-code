using System.CommandLine;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Cli;

public class RunCommandHandler : ICliCommandHandler<RunCliCommand>
{
    private readonly IConsole console;
    private readonly IDayFactory dayFactory;
    private readonly ILoggerFactory loggerFactory;
    private readonly IServiceProvider serviceProvider;
    private readonly IInputService inputService;

    public RunCommandHandler(IConsole console,
        IDayFactory dayFactory,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        IInputService inputService)
    {
        this.console = console;
        this.dayFactory = dayFactory;
        this.loggerFactory = loggerFactory;
        this.serviceProvider = serviceProvider;
        this.inputService = inputService;
    }

    public async Task HandleAsync(RunCliCommand command, CancellationToken cancellationToken)
    {
        var day = this.dayFactory.GetDay(command.Year, command.Day);

        var cancelTask = Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested) { }
        }, cancellationToken);

        var workTask = Task.Run(DayInvoker.InvokeSolve(command.Bonus, day, day.GetType(), this.serviceProvider), cancellationToken);

        var start = Stopwatch.GetTimestamp();

        var rt = await Task.WhenAny(new[]
        {
            workTask,
            cancelTask
        });

        if (rt == cancelTask)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        else if (workTask.IsFaulted)
        {
            throw workTask.Exception!;
        }

        var result = await workTask;

        var elapsed = Stopwatch.GetElapsedTime(start);

        this.console.WriteLine($"Result for {command.Year} day {command.Day}{(command.Bonus ? " (part two)" : " (part one)")} (in {elapsed.TotalMilliseconds:N0} ms):");

        this.console.WriteLine(result.ToString());
    }
}
