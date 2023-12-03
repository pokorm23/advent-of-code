namespace Pokorm.AdventOfCode;

public interface IDayFactory
{
    IDay GetDay(int year, int day);
}
