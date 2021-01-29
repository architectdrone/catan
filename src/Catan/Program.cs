using System;
using System.Collections.Generic;

namespace Catan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Stack<HexagonalCoordinate> hexagonalCoordinates = new Stack<HexagonalCoordinate>(Board.getCoordinatesInBoard(2));
            TileCounts tileCounts = new TileCounts(3, 4, 3, 4, 4, 1);
            Board board = new Board();
            ISet<Board> results = BiomeHelper.getBiomeConfigurationsRecursive(hexagonalCoordinates, tileCounts, board);
            Console.WriteLine($"Found {results.Count} configurations.");
        }
    }


}
