using System;
using System.Collections.Generic;

namespace Catan
{
	/**
	 * Helps with creating biome maps. In biome maps, every tile touches at least one other tile of the same type.
	 */
	public class BiomeHelper
	{
		/**
		 * Returns true if the coordinate meets all constraints.
		 */
		public static bool evaluateCoordinate(Board board, HexagonalCoordinate coord)
        {
			Tile? current = board.getTile(coord);
			if (current == null)
            {
				return true;
            }
			else if (current == Tile.DESERT)
            {
				return true;
            }

			List<Tile?> neighborTiles = new List<Tile?>();

			foreach (HexagonalCoordinate neighborCoord in coord.getNeighbors())
			{
				if (Board.isOnBoard(neighborCoord))
                {
					neighborTiles.Add(board.getTile(neighborCoord));
                }
            }

			return neighborTiles.Contains(current) || neighborTiles.Contains(null);
        }
	}
}

