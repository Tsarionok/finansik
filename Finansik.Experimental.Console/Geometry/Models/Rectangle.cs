namespace Finansik.Experimental.Console.Geometry.Models;

public record Rectangle(long XSide, long YSide) : Square(XSide), IHasSquare
{
    public override long GetSquare => XSide * YSide;

    public override void PrintCoordinatesToConsole()
    {
        base.PrintCoordinatesToConsole();
        System.Console.Write($" [Y:{YSide}]");
    }
}