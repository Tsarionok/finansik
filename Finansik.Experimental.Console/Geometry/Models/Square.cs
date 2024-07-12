namespace Finansik.Experimental.Console.Geometry.Models;

public record Square(long XSide) : IHasSquare
{
    public virtual long GetSquare => XSide * XSide;

    public virtual void PrintCoordinatesToConsole() =>
        System.Console.Write($"[X:{XSide}]");
}