﻿using System.Reflection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Pokorm.AdventOfCode.Tests;

public abstract class DayTestBase : IDisposable
{
    private readonly ConsoleOutput consoleOutput;

    public DayTestBase(ITestOutputHelper output) => this.consoleOutput = new ConsoleOutput(output);

    public IInputService InputService { get; } = new InputService(Mock.Of<IHostEnvironment>(x => x.ContentRootPath == Directory.GetCurrentDirectory()));

    public void Dispose()
    {
        this.consoleOutput.Dispose();
    }

    protected IInputService InputFromSample(string sample)
    {
        return Mock.Of<IInputService>(x => x.GetInput(It.IsAny<int>(), It.IsAny<int>()) == sample);
    }

    protected IDay CreateDay() => CreateDay(this.InputService);

    protected IDay CreateDayFromSample(string sample) => CreateDay(InputFromSample(sample));

    private IDay CreateDay(IInputService service)
    {
        var typeToCreate = GetType().Name.Replace("Tests", string.Empty);

        var types = Assembly.GetExecutingAssembly()
                            .GetReferencedAssemblies()
                            .Select(Assembly.Load)
                            .SelectMany(x => x.GetTypes());

        var type = types.FirstOrDefault(x => x.Name == typeToCreate);

        var day = Activator.CreateInstance(type!, service);

        return (IDay) day!;
    }
}
