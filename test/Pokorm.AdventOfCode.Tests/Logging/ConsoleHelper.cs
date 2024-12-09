namespace Pokorm.AdventOfCode.Tests.Logging;

internal static class ConsoleHelper
{
    public static string InColor(string text, ConsoleColor foreground, ConsoleColor? background = null, bool isBold = false)
    {
        return $"{Color(foreground, background, isBold)}{text}{Reset()}";
    }

    public static string Color(ConsoleColor foreground, ConsoleColor? background = null, bool isBold = false)
    {
        string colorString = "\x1b[";

        if (isBold)
        {
            colorString += "1;";
        }

        switch (foreground)
        {
            case ConsoleColor.Black:
                colorString += "30";

                break;
            case ConsoleColor.DarkBlue:
                colorString += "34";

                break;
            case ConsoleColor.DarkGreen:
                colorString += "32";

                break;
            case ConsoleColor.DarkCyan:
                colorString += "36";

                break;
            case ConsoleColor.DarkRed:
                colorString += "31";

                break;
            case ConsoleColor.DarkMagenta:
                colorString += "35";

                break;
            case ConsoleColor.DarkYellow:
                colorString += "33";

                break;
            case ConsoleColor.Gray:
                colorString += "37";

                break;
            case ConsoleColor.DarkGray:
                colorString += "90";

                break;
            case ConsoleColor.Blue:
                colorString += "94";

                break;
            case ConsoleColor.Green:
                colorString += "92";

                break;
            case ConsoleColor.Cyan:
                colorString += "96";

                break;
            case ConsoleColor.Red:
                colorString += "91";

                break;
            case ConsoleColor.Magenta:
                colorString += "95";

                break;
            case ConsoleColor.Yellow:
                colorString += "93";

                break;
            case ConsoleColor.White:
                colorString += "97";

                break;
            default:
                colorString = "";

                break;
        }

        if (background.HasValue)
        {
            colorString += ";";

            switch (background.Value)
            {
                case ConsoleColor.Black:
                    colorString += "40";

                    break;
                case ConsoleColor.DarkBlue:
                    colorString += "44";

                    break;
                case ConsoleColor.DarkGreen:
                    colorString += "42";

                    break;
                case ConsoleColor.DarkCyan:
                    colorString += "46";

                    break;
                case ConsoleColor.DarkRed:
                    colorString += "41";

                    break;
                case ConsoleColor.DarkMagenta:
                    colorString += "45";

                    break;
                case ConsoleColor.DarkYellow:
                    colorString += "43";

                    break;
                case ConsoleColor.Gray:
                    colorString += "47";

                    break;
                case ConsoleColor.DarkGray:
                    colorString += "100";

                    break;
                case ConsoleColor.Blue:
                    colorString += "104";

                    break;
                case ConsoleColor.Green:
                    colorString += "102";

                    break;
                case ConsoleColor.Cyan:
                    colorString += "106";

                    break;
                case ConsoleColor.Red:
                    colorString += "101";

                    break;
                case ConsoleColor.Magenta:
                    colorString += "105";

                    break;
                case ConsoleColor.Yellow:
                    colorString += "103";

                    break;
                case ConsoleColor.White:
                    colorString += "107";

                    break;
                default:
                    colorString = "";

                    break;
            }
        }

        if (!string.IsNullOrEmpty(colorString))
        {
            colorString += "m";
        }

        return colorString;
    }

    public static string Reset() => "\x1b[0m";
}
