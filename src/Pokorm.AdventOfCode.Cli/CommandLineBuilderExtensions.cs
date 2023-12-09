using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Sharprompt;

namespace Pokorm.AdventOfCode.Cli;

public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseCancelReporting(this CommandLineBuilder builder)
    {
        return builder.AddMiddleware(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException)
            {
                context.Console.Error.WriteLine("Operation was cancelled");

                throw;
            }
        });
    }

    public static CommandLineBuilder UsePromptApi(this CommandLineBuilder builder)
    {
        return builder.AddMiddleware(async (context, next) =>
        {
            Prompt.Culture = CultureInfo.GetCultureInfo("cs-cz");
            Prompt.ThrowExceptionOnCancel = true;

            try
            {
                await next(context);
            }
            catch (PromptCanceledException e)
            {
                var exception = new OperationCanceledException("Prompt cancelled", e);

                throw exception;
            }
        });
    }

    public static CommandLineBuilder UseAppErrorHandling(this CommandLineBuilder builder)
    {
        return builder.AddMiddleware(async (context, next) =>
        {
            try
            {
                try
                {
                    await next(context);
                }
                catch (TargetInvocationException e) when (e.InnerException is not null)
                {
                    ExceptionDispatchInfo.Throw(e.InnerException);

                    throw;
                }
            }
            catch (DeployCliException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                context.Console.Error.WriteLine($"Chyba: {e.Message}");
                Console.ResetColor();
            }
        });
    }

    public static CommandLineBuilder UseTimeout(this CommandLineBuilder builder,
        TimeSpan timeout)
    {
        return builder.AddMiddleware(async (context, next) =>
        {
            var expirationTimeTask = Task.Delay(timeout);
            var pipelineTask = next(context);
            var finishedTask = await Task.WhenAny(expirationTimeTask, pipelineTask);

            if (finishedTask == expirationTimeTask)
            {
                context.Console.Error.WriteLine($"Operation timed out after {timeout.TotalSeconds} seconds");

                throw new OperationCanceledException();
            }
        });
    }
}
