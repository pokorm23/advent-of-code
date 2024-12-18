using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.Rendering;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode.Cli;

var cliBuilder = CliBuilder.Create();

var builder = new CommandLineBuilder(cliBuilder.CreateRootCommand())
              .UseDefaults()
              .UseCancelReporting()
              //.UseTimeout(TimeSpan.FromSeconds(10))
              .UseHost(hostBuilder =>
              {
                  hostBuilder.ConfigureLogging(builder =>
                  {
                      builder.ClearProviders();
                      builder.SetMinimumLevel(LogLevel.Trace);
                      builder.AddSimpleConsole(o => o.SingleLine = true);
                  });

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
