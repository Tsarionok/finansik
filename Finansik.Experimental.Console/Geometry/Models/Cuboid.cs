using System.Text;

namespace Finansik.Experimental.Console.Geometry.Models;

public record Cuboid(long XSide, long Height, long YSide) : Rectangle(XSide, YSide)
{
    public override long GetSquare => 2 * (XSide * Height + XSide * YSide + Height * YSide);

    public override void PrintCoordinatesToConsole()
    {
        System.Console.Write($"[X: {XSide}] [Height: {Height}] [Y: {YSide}] ");
    }
}