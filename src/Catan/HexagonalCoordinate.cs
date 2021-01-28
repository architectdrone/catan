using System;
using System.Collections.Generic;

namespace Catan
{
    class HexagonalCoordinate
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int z { get; private set; }

        public HexagonalCoordinate(int _x, int _y, int _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public List<HexagonalCoordinate> getNeighbors()
        {
            return new List<HexagonalCoordinate> {
                new HexagonalCoordinate(x-1, y, z),
                new HexagonalCoordinate(x+1, y, z),
                new HexagonalCoordinate(x, y-1, z),
                new HexagonalCoordinate(x, y+1, z),
                new HexagonalCoordinate(x, y, z-1),
                new HexagonalCoordinate(x, y, z+1)
            };
        }

        public bool isOrigin()
        {
            return x == 0 && y == 0 && z == 0;
        }
    }
}