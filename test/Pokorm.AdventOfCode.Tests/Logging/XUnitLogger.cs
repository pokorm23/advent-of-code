﻿using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Tests.Logging;

public class XUnitLogger : ILogger
{
    private Func<string?, LogLevel, bool> filter;

    private readonly ITestOutputHelperAccessor outputHelperAccessor;

    public XUnitLogger(string name, ITestOutputHelper outputHelper, XUnitLoggerOptions? options)
        : this(name, new TestOutputHelperAccessor(outputHelper), options) { }

    public XUnitLogger(string name, ITestOutputHelperAccessor accessor, XUnitLoggerOptions? options)
    {
        this.outputHelperAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.filter = options?.Filter ?? (static (_, _) => true);
    }

    public Func<string?, LogLevel, bool> Filter
    {
        get => this.filter;
        set => this.filter = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Name { get; }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        ArgumentNullException.ThrowIfNull(state);

        return XUnitLogScope.Push(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        if (logLevel == LogLevel.None)
        {
            return false;
        }

        return this.Filter(this.Name, logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var message = formatter(state, exception);

        if (!string.IsNullOrEmpty(message) || exception != null)
        {
            WriteMessage(logLevel, eventId.Id, message, exception);
        }
    }

    public virtual void WriteMessage(LogLevel logLevel, int eventId, string? message, Exception? exception)
    {
        var outputHelper = this.outputHelperAccessor?.OutputHelper;

        if (outputHelper is null)
        {
            return;
        }

        var scope = GetScopeInformation();

        var (fg, bg) = logLevel switch
        {
            LogLevel.Critical    => (ConsoleColor.White, (ConsoleColor?) ConsoleColor.DarkRed),
            LogLevel.Error       => (ConsoleColor.Red, null),
            LogLevel.Warning     => (ConsoleColor.Yellow, null),
            LogLevel.Information => (ConsoleColor.Blue, null),
            LogLevel.Debug       => (ConsoleColor.White, null),
            LogLevel.Trace       => (ConsoleColor.Gray, null)
        };

        var line = $"{ConsoleHelper.InColor(scope, ConsoleColor.DarkGray)}{ConsoleHelper.InColor(message, fg, bg, false)}";

        try
        {
            //outputHelper.WriteLine(line);
            outputHelper.WriteLine($"{scope}{message}");

            if (Debugger.IsAttached)
            {
                Trace.WriteLine($"{scope}{message}");
            }
        }
        catch (InvalidOperationException) { }
    }

    private static string GetScopeInformation()
    {
        var builder = new StringBuilder();

        var current = XUnitLogScope.Current;

        var stack = new Stack<XUnitLogScope>();

        while (current != null)
        {
            stack.Push(current);
            current = current.Parent;
        }

        var depth = 0;

        static string DepthPadding(int depth) => new string(' ', depth * 2);

        while (stack.Count > 0)
        {
            var elem = stack.Pop();

            foreach (var property in StringifyScope(elem))
            {
                builder.Append(DepthPadding(depth)).Append(property);
            }

            depth++;
        }

        return builder.ToString();
    }

    private static IEnumerable<string> StringifyScope(XUnitLogScope scope)
    {
        if (scope.State is IEnumerable<KeyValuePair<string, object>> pairs)
        {
            foreach (var pair in pairs)
            {
                yield return $"{pair.Key}: {pair.Value}";
            }
        }
        else if (scope.State is IEnumerable<string> entries)
        {
            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
        else
        {
            yield return scope.ToString();
        }
    }
}
