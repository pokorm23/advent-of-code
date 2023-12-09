using System.Diagnostics;
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
        Trace.Listeners.Add(new TextWriterTraceListener(this.stringWriter));
        Console.SetOut(this.stringWriter);
    }

    public void Dispose()
    {
        try
        {
            var capture = this.stringWriter.ToString();

            foreach (var s in capture.ReplaceLineEndings().Split(Environment.NewLine))
            {
                this.output.WriteLine(s);
            }
        }
        catch 
        {
        }

        Console.SetOut(this.originalOutput);
        this.stringWriter.Dispose();
    }
}
