using System.Reflection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Pokorm.AdventOfCode.Tests;

public abstract class DayTestBase : IDisposable
{
    public ITestOutputHelper Output { get; }

    private readonly ConsoleOutput consoleOutput;

    public DayTestBase(ITestOutputHelper output)
    {
        this.Output = output;
        this.consoleOutput = new ConsoleOutput(output);
    }

    public IInputService InputService { get; } = new InputService(Mock.Of<IHostEnvironment>(x => x.ContentRootPath == Directory.GetCurrentDirectory()));

    public void Dispose()
    {
        this.consoleOutput.Dispose();
    }

    protected IInputService InputFromSample(string sample)
    {
        return Mock.Of<IInputService>(x => x.GetInput(It.IsAny<int>(), It.IsAny<int>()) == sample);
    }

    protected string[] LinesFromSample(string sample)
    {
        return sample.ReplaceLineEndings().Split(Environment.NewLine, StringSplitOptions.TrimEntries);
    }

    protected string[] LinesForDay<T>(T day) where T : class
    {
        return this.InputService.GetInputLines(day.GetType());
    }

    protected string TextForDay<T>(T day) where T : class
    {
        return this.InputService.GetInput(day.GetType());
    }
}
