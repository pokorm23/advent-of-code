using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.Rendering;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokorm.AdventOfCode.Cli;

var cliBuilder = CliBuilder.Create();

var builder = new CommandLineBuilder(cliBuilder.CreateRootCommand())
              .UseDefaults()
              .UseCancelReporting()
              .UseHost(hostBuilder =>
              {
                  var cliContext = hostBuilder.Properties[typeof(InvocationContext)] as InvocationContext;

                  hostBuilder.ConfigureServices((_, services) =>
                  {
                      services.AddAdventOfCodeApp();

                      if (cliContext is not null)
                      {
                          services.TryAddSingleton(new ConsoleRenderer(cliContext.Console, resetAfterRender: true));
                      }
                  });
              })
              .UsePromptApi()
              .UseAppErrorHandling();

var app = builder.Build();

return await app.InvokeAsync(args);
