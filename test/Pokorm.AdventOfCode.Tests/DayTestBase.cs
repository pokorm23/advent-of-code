using Microsoft.Extensions.Hosting;
using Moq;

namespace Pokorm.AdventOfCode.Tests;

public abstract class DayTestBase
{
    public IInputService InputService { get; } = new InputService(Mock.Of<IHostEnvironment>(x => x.ContentRootPath == Directory.GetCurrentDirectory()));

    protected IInputService InputFromSample(string sample)
    {
        return Mock.Of<IInputService>(x => x.GetInput(It.IsAny<int>(), It.IsAny<int>()) == sample);
    }
}
