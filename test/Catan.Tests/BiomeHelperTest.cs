using System;
using System.Collections.Generic;
using Xunit;

namespace Catan.Tests
{
    public class BiomeHelperTest
    {
        public class EvaluatorTest
        {
            [Theory]
            [InlineData(0, 0, 0, 1, 0, -1)]
            [InlineData(0, 0, 0, -1, 0, 1)]
            [InlineData(0, 0, 0, 0, 1, -1)]
            [InlineData(0, 0, 0, 0, -1, 1)]
            [InlineData(0, 0, 0, 1, -1, 0)]
            [InlineData(0, 0, 0, -1, 1, 0)]
            [InlineData(2, 0, -2, 1, 0, -1)]
            [InlineData(2, 0, -2, 2, -1, -1)]
            [InlineData(2, 0, -2, 1, 1, -2)]
            public void returnsTrueIfHasOneSurrounding(int x, int y, int z, int ox, int oy, int oz)
            {
                Board board = new Board();
                board.setTile(new HexagonalCoordinate(x, y, z), Tile.CLAY);
                board.setTile(new HexagonalCoordinate(ox, oy, oz), Tile.CLAY);
                Assert.True(BiomeHelper.evaluateCoordinate(board, new HexagonalCoordinate(x, y, z)));
            }
        }
    }
}
