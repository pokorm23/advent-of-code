using System.CommandLine.IO;

namespace Pokorm.AdventOfCode.Cli;

public static class StandardStreamWriterExtensions
{
    public static void WriteLineColor(this IStandardStreamWriter writer, string content, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        writer.WriteLine(content);
        Console.ResetColor();
    }

    public static void WriteColor(this IStandardStreamWriter writer, string content, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        writer.Write(content);
        Console.ResetColor();
    }
}
