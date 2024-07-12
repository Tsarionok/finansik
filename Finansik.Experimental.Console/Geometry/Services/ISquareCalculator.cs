using Finansik.Experimental.Console.Geometry.Models;

namespace Finansik.Experimental.Console.Geometry.Services;

public interface ISquareCalculator<in TSquare> where TSquare : Square
{
    long CalculateSquare(TSquare square);
}

internal class SquareCalculator : ISquareCalculator<Square>
{
    public long CalculateSquare(Square square) => square.XSide * square.XSide;
}

internal class RectangleSquareCalculator : ISquareCalculator<Rectangle>
{
    public long CalculateSquare(Rectangle rect) => rect.XSide * rect.YSide;
}

internal class CuboidSquareCalculator : ISquareCalculator<Cuboid>
{
    public long CalculateSquare(Cuboid cuboid) => 
        2 * (cuboid.XSide * cuboid.YSide + cuboid.XSide * cuboid.Height + cuboid.YSide * cuboid.Height);
}