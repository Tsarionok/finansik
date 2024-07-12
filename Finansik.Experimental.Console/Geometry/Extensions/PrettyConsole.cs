namespace Finansik.Experimental.Console.Geometry.Extensions;

public static class PrettyConsole
{
    public static void Write(string text, ConsoleColor color)
    {
        var currentColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = color;
        System.Console.Write(text);
        System.Console.ForegroundColor = currentColor;
    }
}