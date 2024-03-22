using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pokorm.AdventOfCode.Cli;

public class CliBuilder
{
    public static CliBuilder Create()
    {
        var services = new ServiceCollection();

        services.AddSingleton<CliBuilder>();

        var cliBuilder = services.BuildServiceProvider().GetRequiredService<CliBuilder>();

        return cliBuilder;
    }

    public RootCommand CreateRootCommand()
    {
        var currentYear = DateTime.UtcNow.AddHours(-5).AddMonths(1).Year - 1;

        var rootCommand = new RootCommand
        {
            new Argument<int>("day", "Den adventu")
            {
                Arity = ArgumentArity.ExactlyOne
            },
            new Option<bool>(new[]
            {
                "-b",
                "--bonus"
            }, "Bonusová část")
            {
                Arity = ArgumentArity.ZeroOrOne
            },
            new Option<int>(new[]
            {
                "-y",
                "--year"
            }, () => currentYear, "Rok adventu")
            {
                Arity = ArgumentArity.ZeroOrOne
            }
        };

        rootCommand.Name = "[Pokorm.AdventOfCode.Cli]";

        rootCommand.Handler = CommandHandler.Create(CreateHandler<RunCliCommand, RunCommandHandler>);

        return rootCommand;
    }
    
    public static ICommandHandler CreateCliCommandHandler<TCommand, THandler>()
        where TCommand : class
        where THandler : ICliCommandHandler<TCommand>
    {
        return CommandHandler.Create(CreateHandler<TCommand, THandler>);
    }

    public static async Task CreateHandler<TCommand, THandler>(TCommand command, InvocationContext context, IHost host, CancellationToken cancellationToken)
        where TCommand : class
        where THandler : ICliCommandHandler<TCommand>
    {
        context.Console.Out.WriteLineColor(JsonSerializer.Serialize(command), ConsoleColor.DarkGray);

        var di = host.Services;

        await using var scope = di.CreateAsyncScope();

        var handler = scope.ServiceProvider.GetRequiredService<THandler>();

        await handler.HandleAsync(command, cancellationToken);
    }
}
