using Xunit.Abstractions;

namespace Pokorm.AdventOfCode.Tests;

public class ConsoleOutput : IDisposable
{
    private readonly TextWriter originalOutput;
    private readonly ITestOutputHelper output;
    private readonly StringWriter stringWriter;

    public ConsoleOutput(ITestOutputHelper output)
    {
        this.output = output;
        this.stringWriter = new StringWriter();
        this.originalOutput = Console.Out;
        Console.SetOut(this.stringWriter);
    }

    public void Dispose()
    {
        var capture = this.stringWriter.ToString();

        foreach (var s in capture.ReplaceLineEndings().Split(Environment.NewLine))
        {
            this.output.WriteLine(s);
        }

        Console.SetOut(this.originalOutput);
        this.stringWriter.Dispose();
    }
}
