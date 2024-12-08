namespace Pokorm.AdventOfCore.Benchmarks;

public record DayBenchmarkParam(Type Type, Func<object> SolveFunc)
{
    public override string ToString()
    {
        return $"{Type.Namespace!.Split('.')[^2][1..]}.{int.Parse(Type.Name[4..])}";
    }
}
