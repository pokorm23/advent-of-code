using BenchmarkDotNet.Attributes;

namespace Pokorm.AdventOfCore.Benchmarks;

[InProcess]
[MemoryDiagnoser]
public class DayBenchmarks
{
    public static List<(Type x, object?, Func<object>, Func<object>)> Days { get; set; }

    public IEnumerable<object> SolveParams()
    {
        foreach (var (type, obj, solve, solveBonus) in Days)
        {
            yield return new DayBenchmarkParam(type, solve);
        }
    }

    public IEnumerable<object> SolveBonusParams()
    {
        foreach (var (type, obj, solve, solveBonus) in Days)
        {
            yield return new DayBenchmarkParam(type, solveBonus);
        }
    }

    [Benchmark]
    [ArgumentsSource(nameof(SolveParams))]
    public object Solve(DayBenchmarkParam param) => param.SolveFunc();

    [Benchmark]
    [ArgumentsSource(nameof(SolveBonusParams))]
    public object SolveBonus(DayBenchmarkParam param) => param.SolveFunc();
}
