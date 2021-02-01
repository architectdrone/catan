using System;
using System.Collections.Generic;

namespace Catan
{
    public class HexagonalCoordinate
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int z { get; private set; }

        public HexagonalCoordinate(int _x, int _y, int _z)
        {
            if (_x + _y + _z != 0)
            {
                throw new ArithmeticException();
            }

            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public HexagonalCoordinate[] getNeighbors()
        {
            return new HexagonalCoordinate[] {
                new HexagonalCoordinate(x-1, y, z+1),
                new HexagonalCoordinate(x+1, y, z-1),
                new HexagonalCoordinate(x, y-1, z+1),
                new HexagonalCoordinate(x, y+1, z-1),
                new HexagonalCoordinate(x-1, y+1, z),
                new HexagonalCoordinate(x+1, y-1, z)
            };
        }

        public bool isOrigin()
        {
            return x == 0 && y == 0 && z == 0;
        }

        public override bool Equals(object obj)
        {
            return obj is HexagonalCoordinate coordinate &&
                   x == coordinate.x &&
                   y == coordinate.y &&
                   z == coordinate.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }
    }
}