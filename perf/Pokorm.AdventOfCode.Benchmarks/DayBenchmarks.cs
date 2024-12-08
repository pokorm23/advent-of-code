using BenchmarkDotNet.Attributes;

namespace Pokorm.AdventOfCore.Benchmarks;

[InProcess]
[MemoryDiagnoser]
public class DayBenchmarks
{
    public static List<(Type x, object?, Func<long>, Func<long>)> Days { get; set; }

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
    public long Solve(DayBenchmarkParam param) => param.SolveFunc();

    //[Benchmark]
    //[ArgumentsSource(nameof(SolveBonusParams))]
    public long SolveBonus(DayBenchmarkParam param) => param.SolveFunc();
}
