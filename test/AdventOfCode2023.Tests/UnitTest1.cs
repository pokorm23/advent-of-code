using Microsoft.Extensions.Hosting;
using Moq;
using Pokorm.AdventOfCode2023;

namespace AdventOfCode2023.Tests;

public class DayRegressionTests
{
    private readonly IInputService inputService;

    public DayRegressionTests()
    {
        this.inputService = new InputService(Mock.Of<IHostEnvironment>(
            x => x.ContentRootPath == Directory.GetCurrentDirectory()));
    }

    [Fact]
    public async Task Day1_Sample1()
    {
        var day = new Day1(Mock.Of<IInputService>(
            x => x.GetOrDownloadInputAsync(It.Is<int>(y => y == 1)
                 ) == Task.FromResult(
                     """
                     1abc2
                     pqr3stu8vwx
                     a1b2c3d4e5f
                     treb7uchet
                     """
                 )));

        var result = await day.SolveAsync();

        Assert.Equal("142", result);
    }

    [Fact]
    public async Task Day1()
    {
        var day = new Day1(this.inputService);

        var result = await day.SolveAsync();

        Assert.Equal("55447", result);
    }

    [Fact]
    public async Task Day1_Sample2()
    {
        var day = new Day1(Mock.Of<IInputService>(
            x => x.GetOrDownloadInputAsync(It.Is<int>(y => y == 1)
                 ) == Task.FromResult(
                     """
                     two1nine
                     eightwothree
                     abcone2threexyz
                     xtwone3four
                     4nineeightseven2
                     zoneight234
                     7pqrstsixteen
                     """
                 )));

        var result = await day.SolveBonusAsync();

        Assert.Equal("281", result);
    }

    [Fact]
    public async Task Day1_Bonus()
    {
        var day = new Day1(this.inputService);

        var result = await day.SolveBonusAsync();

        Assert.Equal("54706", result);
    }
}
