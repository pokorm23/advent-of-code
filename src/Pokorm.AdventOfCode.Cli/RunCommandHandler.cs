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
    private readonly IInputService inputService;

    public RunCommandHandler(IConsole console,
        IDayFactory dayFactory,
        ILoggerFactory loggerFactory,
        IInputService inputService)
    {
        this.console = console;
        this.dayFactory = dayFactory;
        this.loggerFactory = loggerFactory;
        this.inputService = inputService;
    }

    public async Task HandleAsync(RunCliCommand command, CancellationToken cancellationToken)
    {
        var day = this.dayFactory.GetDay(command.Year, command.Day);

        var cancelTask = Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested) { }
        }, cancellationToken);

        var workTask = Task.Run(() => RunTask(command.Bonus, day), cancellationToken);

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

    private long RunTask<T>(bool bonus, T day) where T : class
    {
        if (day is IDay d)
        {
            return bonus ? d.SolveBonus() : d.Solve();
        }

        var type = day.GetType();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                          .Where(x => x.Name.Contains("solve", StringComparison.OrdinalIgnoreCase))
                          .ToList();

        var bonusMethods = methods.SingleOrDefault(x => x.Name.Contains("bonus", StringComparison.OrdinalIgnoreCase)
                                                        || x.Name.Contains("parttwo", StringComparison.OrdinalIgnoreCase));

        if (bonus)
        {
            if (bonusMethods is not null)
            {
                return (long) (bonusMethods.Invoke(day, GetParameters(bonusMethods).ToArray()) ?? 0);
            }
        }

        var partOneMethod = methods.SingleOrDefault(x => x != bonusMethods) ?? throw new Exception("partOneMethod not found");

        return (long) (partOneMethod.Invoke(day, GetParameters(partOneMethod).ToArray()) ?? 0);

        IEnumerable<object?> GetParameters(MethodInfo method)
        {
            foreach (var parameterInfo in method.GetParameters())
            {
                if (parameterInfo.ParameterType == typeof(string))
                {
                    yield return this.inputService.GetInput(type);
                }

                if (parameterInfo.ParameterType == typeof(string[]))
                {
                    yield return this.inputService.GetInputLines(type);
                }

                else if (parameterInfo.ParameterType == typeof(IConsole))
                {
                    yield return this.console;
                }

                else if (parameterInfo.ParameterType == typeof(ILoggerFactory))
                {
                    yield return this.loggerFactory;
                }

                else if (parameterInfo.ParameterType == typeof(ILogger<>).MakeGenericType(type))
                {
                    var ctr = typeof(Logger<>).MakeGenericType(type).GetConstructor([ typeof(ILoggerFactory) ]);

                    yield return ctr!.Invoke([ this.loggerFactory ]);
                }

                else
                {
                    throw new Exception(parameterInfo.ToString());
                }
            }
        }
    }
}
