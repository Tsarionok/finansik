using Finansik.Experimental.Console.Geometry.Extensions;
using Finansik.Experimental.Console.Geometry.Models;

namespace Finansik.Experimental.Console.Geometry.Services;

public class FigureService<TSquare>(ISquareCalculator<TSquare> squareCalculator) 
    : IFigureService<TSquare> where TSquare : Square
{
    public void PrintFigureInfo(TSquare figure)
    {
        PrintFigureType(figure);
        System.Console.WriteLine(squareCalculator.CalculateSquare(figure));
        // figure.PrintCoordinatesToConsole();
        // System.Console.WriteLine(figure.GetSquare);
    }

    private void PrintFigureType(Square figure)
    {
        PrettyConsole.Write($"[{figure.GetType().Name}]\t", ConsoleColor.Green);
    }
}