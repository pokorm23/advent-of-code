using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pokorm.AdventOfCode;

public static class DayInvoker
{
    public static Func<object> InvokeSolve(bool bonus, object? day, Type type, IServiceProvider serviceProvider)
    {
        if (day is IDay d)
        {
            return bonus ? () => d.SolveBonus() : () => d.Solve();
        }

        var methods = type.GetMethods()
                          .Where(x => x.Name.Contains("solve", StringComparison.OrdinalIgnoreCase))
                          .ToList();

        var bonusMethods = methods.SingleOrDefault(x => x.Name.Contains("bonus", StringComparison.OrdinalIgnoreCase)
                                                        || x.Name.Contains("parttwo", StringComparison.OrdinalIgnoreCase));

        MethodInfo? handleMethod = null;
        
        if (bonus)
        {
            if (bonusMethods is not null)
            {
                handleMethod = bonusMethods;
            }
        }
        
        if (handleMethod is null)
        {
            handleMethod= methods.SingleOrDefault(x => x != bonusMethods) ?? throw new Exception("partOneMethod not found");
        }

        return InvokeExecuteMethod(handleMethod, day, type, serviceProvider);
    }

    private static Func<object> InvokeExecuteMethod(MethodInfo handleMethod,
        object? instance,
        Type type,
        IServiceProvider serviceProvider)
    {
        var paramValues = new List<object>();
        var inputService = serviceProvider.GetRequiredService<IInputService>();

        if (handleMethod.IsStatic)
        {
            instance = null;
        }

        foreach (var parameterInfo in handleMethod.GetParameters())
        { 
            if (parameterInfo.ParameterType == typeof(string))
            {
                paramValues.Add(inputService.GetInput(type));
            }
            else if (parameterInfo.ParameterType == typeof(string[]))
            {
                paramValues.Add(inputService.GetInputLines(type));
            }
            else
            {
                var service = serviceProvider.GetService(parameterInfo.ParameterType);

                if (service is null)
                {
                    throw new InvalidOperationException($"Could not resolve service of type {parameterInfo.ParameterType.Name}");
                }

                paramValues.Add(service);
            }
        }

        if (handleMethod.ReturnType.IsAssignableTo(typeof(long)))
        {
            return () => (long) handleMethod.Invoke(instance, paramValues.ToArray())!;
        }
        else if (handleMethod.ReturnType.IsAssignableTo(typeof(string)))
        {
            return () => (string) handleMethod.Invoke(instance, paramValues.ToArray())!;
        }
        else
        {
            throw new InvalidOperationException("Handler method must return Task, Task<int>, ValueTask, ValueTask<int> or int");
        }
    }
}
