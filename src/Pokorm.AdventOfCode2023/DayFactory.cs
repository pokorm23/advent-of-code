namespace Pokorm.AdventOfCode2023;

internal class DayFactory : IDayFactory
{
    private readonly IEnumerable<IDay> days;

    public DayFactory(IEnumerable<IDay> days) => this.days = days;

    public IDay GetDay(int day)
    {
        var dayInstance = this.days.Where(x => x.Day == day).ToList();

        if (dayInstance.Count != 1)
        {
            throw new Exception($"Day {day} not found.");
        }

        if (dayInstance.Count > 1)
        {
            throw new Exception($"Multiple days with number {day} found.");
        }

        return dayInstance[0];
    }
}
