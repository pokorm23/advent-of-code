using Microsoft.Extensions.Hosting;
using Moq;
using Xunit.Abstractions;

namespace Pokorm.AdventOfCode.Tests;

public abstract class DayTestBase : IDisposable
{
    private readonly ConsoleOutput consoleOutput;

    public DayTestBase(ITestOutputHelper output)
    {
        consoleOutput = new ConsoleOutput(output);
    }
    
    public IInputService InputService { get; } = new InputService(Mock.Of<IHostEnvironment>(x => x.ContentRootPath == Directory.GetCurrentDirectory()));

    protected IInputService InputFromSample(string sample)
    {
        return Mock.Of<IInputService>(x => x.GetInput(It.IsAny<int>(), It.IsAny<int>()) == sample);
    }

    public void Dispose()
    {
        this.consoleOutput.Dispose();
    }
}