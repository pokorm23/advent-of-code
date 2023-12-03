using System.Text.RegularExpressions;

namespace Pokorm.AdventOfCode;

internal class DayFactory : IDayFactory
{
    private readonly IEnumerable<IDay> days;

    public DayFactory(IEnumerable<IDay> days) => this.days = days;

    public IDay GetDay(int year, int day)
    {
        var dayInstance = this.days.Where(x =>
        {
            var type = x.GetType();

            if (!(type.Namespace?.Contains(year.ToString()) ?? false))
            {
                return false;
            }
            
            return Regex.IsMatch(type.Name, $"^Day{day.ToString().PadLeft(2, '0')}$");
        }).ToList();

        if (dayInstance.Count != 1)
        {
            throw new Exception($"Day {day} of {year} not found.");
        }

        if (dayInstance.Count > 1)
        {
            throw new Exception($"Multiple days with number {day} for {year} found.");
        }

        return dayInstance[0];
    }
}
