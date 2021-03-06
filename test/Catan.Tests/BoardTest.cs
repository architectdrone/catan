using System;
using System.Collections.Generic;
using Xunit;

namespace Catan.Tests
{
    public class BoardTest
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 7)]
        [InlineData(2, 19)]
        [InlineData(3, 37)]
        [InlineData(4, 61)]
        public void tilesInBoardWorks(int size, int expected)
        {
            Assert.Equal(expected, Board.tilesInBoard(size));
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, -1, 0, 1)]
        [InlineData(1, 0, -1, 2)]
        [InlineData(0, 1, -1, 3)]
        [InlineData(-1, 1, 0, 4)]
        [InlineData(-1, 0, 1, 5)]
        [InlineData(0, -1, 1, 6)]
        [InlineData(2, -2, 0, 7)]
        [InlineData(2, -1, -1, 8)]
        [InlineData(2, 0, -2, 9)]
        [InlineData(1, 1, -2, 10)]
        [InlineData(0, 2, -2, 11)]
        [InlineData(-1, 2, -1, 12)]
        [InlineData(-2, 2, 0, 13)]
        [InlineData(-2, 1, 1, 14)]
        [InlineData(-2, 0, 2, 15)]
        [InlineData(-1, -1, 2, 16)]
        [InlineData(0, -2, 2, 17)]
        [InlineData(1, -2, 1, 18)]
        public void indexWorks(int x, int y, int z, int expected)
        {

            Assert.Equal(expected, Board.getIndex(new HexagonalCoordinate(x, y, z)));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, -1, 0)]
        [InlineData(1, 0, -1)]
        [InlineData(0, 1, -1)]
        [InlineData(-1, 1, 0)]
        [InlineData(-1, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(2, -2, 0)]
        [InlineData(2, -1, -1)]
        [InlineData(2, 0, -2)]
        [InlineData(1, 1, -2)]
        [InlineData(0, 2, -2)]
        [InlineData(-1, 2, -1)]
        [InlineData(-2, 2, 0)]
        [InlineData(-2, 1, 1)]
        [InlineData(-2, 0, 2)]
        [InlineData(-1, -1, 2)]
        [InlineData(0, -2, 2)]
        [InlineData(1, -2, 1)]
        public void settingATileWorks(int x, int y, int z)
        {
            Board board = new Board();
            board.setTile(new HexagonalCoordinate(x, y, z), Tile.CLAY);
            Assert.Equal(Tile.CLAY, board.getTile(new HexagonalCoordinate(x, y, z)));
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, -1, 0, 1)]
        [InlineData(1, 0, -1, 1)]
        [InlineData(0, 1, -1, 1)]
        [InlineData(-1, 1, 0, 1)]
        [InlineData(-1, 0, 1, 1)]
        [InlineData(0, -1, 1, 1)]
        [InlineData(2, -2, 0, 2)]
        [InlineData(2, -1, -1, 2)]
        [InlineData(2, 0, -2, 2)]
        [InlineData(1, 1, -2, 2)]
        [InlineData(0, 2, -2, 2)]
        [InlineData(-1, 2, -1, 2)]
        [InlineData(-2, 2, 0, 2)]
        [InlineData(-2, 1, 1, 2)]
        [InlineData(-2, 0, 2, 2)]
        [InlineData(-1, -1, 2, 2)]
        [InlineData(0, -2, 2, 2)]
        [InlineData(1, -2, 1, 2)]
        public void indexToRingWorks(int x, int y, int z, int correctRingNumber)
        {
            int index = Board.getIndex(new HexagonalCoordinate(x, y, z));
            Assert.Equal(correctRingNumber, Board.indexToRing(index));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, -1, 0)]
        [InlineData(1, 0, -1)]
        [InlineData(0, 1, -1)]
        [InlineData(-1, 1, 0)]
        [InlineData(-1, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(2, -2, 0)]
        [InlineData(2, -1, -1)]
        [InlineData(2, 0, -2)]
        [InlineData(1, 1, -2)]
        [InlineData(0, 2, -2)]
        [InlineData(-1, 2, -1)]
        [InlineData(-2, 2, 0)]
        [InlineData(-2, 1, 1)]
        [InlineData(-2, 0, 2)]
        [InlineData(-1, -1, 2)]
        [InlineData(0, -2, 2)]
        [InlineData(1, -2, 1)]
        public void indexToHexCoordWorks(int x, int y, int z)
        {
            var coord = new HexagonalCoordinate(x, y, z);
            int index = Board.getIndex(coord);
            Assert.Equal(coord, Board.indexToCoordinate(index));
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, -1, 0, 1)]
        [InlineData(1, 0, -1, 1)]
        [InlineData(0, 1, -1, 1)]
        [InlineData(-1, 1, 0, 1)]
        [InlineData(-1, 0, 1, 1)]
        [InlineData(0, -1, 1, 1)]
        [InlineData(2, -2, 0, 2)]
        [InlineData(2, -1, -1, 2)]
        [InlineData(2, 0, -2, 2)]
        [InlineData(1, 1, -2, 2)]
        [InlineData(0, 2, -2, 2)]
        [InlineData(-1, 2, -1, 2)]
        [InlineData(-2, 2, 0, 2)]
        [InlineData(-2, 1, 1, 2)]
        [InlineData(-2, 0, 2, 2)]
        [InlineData(-1, -1, 2, 2)]
        [InlineData(0, -2, 2, 2)]
        [InlineData(1, -2, 1, 2)]
        public void coordinatesInRingWorks(int x, int y, int z, int ringNumber)
        {
            ISet<HexagonalCoordinate> coordinatesInRing = Board.getCoordinatesInRing(ringNumber);
            Assert.Equal(ringNumber != 0 ? 6 * ringNumber : 1, coordinatesInRing.Count);
            Assert.Contains(new HexagonalCoordinate(x, y, z), coordinatesInRing);
        }

        [Theory]
        [InlineData(0, 0, 0, 2)]
        [InlineData(1, -1, 0, 2)]
        [InlineData(1, 0, -1, 2)]
        [InlineData(0, 1, -1, 2)]
        [InlineData(-1, 1, 0, 2)]
        [InlineData(-1, 0, 1, 2)]
        [InlineData(0, -1, 1, 2)]
        [InlineData(2, -2, 0, 2)]
        [InlineData(2, -1, -1, 2)]
        [InlineData(2, 0, -2, 2)]
        [InlineData(1, 1, -2, 2)]
        [InlineData(0, 2, -2, 2)]
        [InlineData(-1, 2, -1, 2)]
        [InlineData(-2, 2, 0, 2)]
        [InlineData(-2, 1, 1, 2)]
        [InlineData(-2, 0, 2, 2)]
        [InlineData(-1, -1, 2, 2)]
        [InlineData(0, -2, 2, 2)]
        [InlineData(1, -2, 1, 2)]
        public void coordinatesInBoardWorks(int x, int y, int z, int boardSize)
        {
            ISet<HexagonalCoordinate> coordinatesInBoard = Board.getCoordinatesInBoard(boardSize);
            Assert.Equal(Board.tilesInBoard(boardSize), coordinatesInBoard.Count);
            Assert.Contains(new HexagonalCoordinate(x, y, z), coordinatesInBoard);
        }

        [Fact]
        public void settingATileOffOfTheBoardDoesntWork()
        {
            Board board = new Board();
            Assert.Throws<ArithmeticException>(() => { board.setTile(new HexagonalCoordinate(3, -1, -2), Tile.CLAY); });
        }

        [Fact]
        public void gettingATileOffOfTheBoardDoesntWork()
        {
            Board board = new Board();
            Assert.Throws<ArithmeticException>(() => { board.getTile(new HexagonalCoordinate(3, -1, -2)); });
        }
    }
}
