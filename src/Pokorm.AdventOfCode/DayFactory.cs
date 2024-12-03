using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Pokorm.AdventOfCode;

internal class DayFactory : IDayFactory
{
    private readonly IServiceProvider serviceProvider;
    private IServiceCollection? services;

    public DayFactory(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

    public object GetDay(int year, int day)
    {
        var dayInstance = GetServices().Select(x => x.ImplementationType!).Where(type =>
        {
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

        return ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, dayInstance[0]);
    }

    private IServiceCollection GetServices()
    {
        if (this.services is not null)
        {
            return this.services;
        }

        this.services = new ServiceCollection();

        this.services.Scan(scan => scan
                                   .FromAssemblyOf<IDay>()
                                   .AddClasses(classes => classes.Where(t => t.Name.StartsWith("Day")))
                                   .AsSelf()
                                   .WithSingletonLifetime());

        return this.services;
    }
}
