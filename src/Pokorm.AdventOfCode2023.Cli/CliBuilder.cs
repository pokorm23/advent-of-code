using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pokorm.AdventOfCode2023.Cli;

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
        var runCommand = new Command("run")
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
            }
        };

        var rootCommand = new RootCommand
        {
            runCommand
        };

        rootCommand.Name = "[Pokorm.AdventOfCode2023.Cli]";

        runCommand.Handler = CommandHandler.Create((RunCliCommand command, InvocationContext context, IHost host, CancellationToken cancellationToken) =>
        {
            context.Console.Out.WriteLineColor(JsonSerializer.Serialize(command), ConsoleColor.DarkGray);

            var di = host.Services;

            using var scope = di.CreateAsyncScope();

            var handler = scope.ServiceProvider.GetRequiredService<RunCommandHandler>();

            return handler.HandleAsync(command, cancellationToken);
        });

        return rootCommand;
    }
}
