namespace Pokorm.AdventOfCode;

public interface IDayFactory
{
    object GetDay(int year, int day);

    IEnumerable<Type> GetAllDayTypes();

    object? TryCreateDay(Type type);
}
