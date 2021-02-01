using System;
using System.Collections.Generic;

namespace Catan
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Hello World!");
            Stack<HexagonalCoordinate> hexagonalCoordinates = new Stack<HexagonalCoordinate>(Board.getCoordinatesInBoard(Board.SIZE));
            TileCounts tileCounts = new TileCounts(3, 4, 3, 4, 4, 1);
            Board board = new Board();
            watch.Start();
            //List<Board> results = BiomeHelper.getBiomeConfigurationsRecursive(hexagonalCoordinates, tileCounts, board);
            List<Board> results = BiomeHelper.getBiomeConfigurationsRecursive(0, Board.tilesInBoard(Board.SIZE), tileCounts, board);
            watch.Stop();
            Console.WriteLine($"Found {results.Count} configurations in {watch.Elapsed.TotalSeconds}s.");
        }
    }
}
