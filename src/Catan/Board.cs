using System;
using System.Collections.Generic;

namespace Catan
{
    class Board
    {
        private List<Tile> boardState;

        /**
        * Returns true if the given coordinates exist on the board.
        */
        public bool isDefined(HexagonalCoordinate coordinate)
        {
            return (coordinate.x <= 2 && coordinate.y <= 2 && coordinate.z <= 2);
        }
    }

    enum Tile
    {
        WOOD,
        STONE,
        SHEEP,
        WHEAT,
        CLAY,
        DESERT
    }
}