using Finansik.Experimental.Console.Geometry.Models;

namespace Finansik.Experimental.Console.Geometry.Services;

public interface IFigureService<in TSquare> where TSquare : Square
{
    void PrintFigureInfo(TSquare figure);
}