using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode;
using Pokorm.AdventOfCore.Benchmarks;

var services = new ServiceCollection();

services.AddLogging(b =>
{
    b.ClearProviders();
});

services.AddSingleton<IHostEnvironment>(new HostingEnvironment()
{
    ContentRootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location)!
});

services.AddAdventOfCode();

var di = services.BuildServiceProvider();

var f = di.GetRequiredService<IDayFactory>();

DayBenchmarks.Days = f
                     .GetAllDayTypes()
                     .Select(x => (x, f.TryCreateDay(x)))
                     .Select(x => (x.Item1, x.Item2,
                                   DayInvoker.InvokeSolve(false, x.Item2, x.Item1, di),
                                   DayInvoker.InvokeSolve(true, x.Item2, x.Item1, di)))
                     .ToList();

_ = BenchmarkRunner.Run<DayBenchmarks>(DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true));
