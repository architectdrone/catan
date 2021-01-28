using System;
using System.Collections.Generic;
using Xunit;

namespace Catan.Tests
{
    public class HexagonalCoordinateTest
    {
        [Fact]
        public void coordinateAt0_0_0IsOrigin()
        {
            HexagonalCoordinate coordinate = new HexagonalCoordinate(0, 0, 0);
            Assert.True(coordinate.isOrigin());
        }

        [Fact]
        public void coordinateNotAt0_0_0IsNotOrigin()
        {
            HexagonalCoordinate coordinate = new HexagonalCoordinate(0, 1, -1);
            Assert.False(coordinate.isOrigin());
        }

        [Fact]
        public void getNeighborsWorks()
        {
            HexagonalCoordinate coordinate = new HexagonalCoordinate(0, 1, -1);

            Assert.Contains(new HexagonalCoordinate(0, 2, -2), coordinate.getNeighbors());
            Assert.Contains(new HexagonalCoordinate(-1, 2, -1), coordinate.getNeighbors());
            Assert.Contains(new HexagonalCoordinate(-1, 1, 0), coordinate.getNeighbors());
            Assert.Contains(new HexagonalCoordinate(0, 0, 0), coordinate.getNeighbors());
            Assert.Contains(new HexagonalCoordinate(1, 0, -1), coordinate.getNeighbors());
            Assert.Contains(new HexagonalCoordinate(1, 1, -2), coordinate.getNeighbors());
            Assert.Equal(coordinate.getNeighbors().Count, 6);
        }
    }
}
