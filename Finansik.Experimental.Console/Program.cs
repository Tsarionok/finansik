using Finansik.Experimental.Console.Geometry;
using Finansik.Experimental.Console.Geometry.Models;
using Finansik.Experimental.Console.Geometry.Services;

var figureService = new FigureService<Rectangle>(new SquareCalculator());

var figures = new[]
{
    new Square(3),
    new Square(10),
    new Rectangle(2, 10),
    new Rectangle(4, 20),
    new Cuboid(2, 3, 10)
};

foreach (var hasSquare in figures)
{
    figureService.PrintFigureInfo(new Cuboid(2, 4, 5));
}



