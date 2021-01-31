﻿using System;
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

		public static bool evaluateNeighboringCoordinates(Board board, HexagonalCoordinate coord)
        {
            foreach (var neighboringCoord in coord.getNeighbors())
            {
				if (Board.isOnBoard(neighboringCoord))
                {
					if (!evaluateCoordinate(board, neighboringCoord))
                    {
						return false;
                    }
                }
            }
			return evaluateCoordinate(board, coord);
        }

		private static Board? getNextValidState(Tile tile, HexagonalCoordinate coord, Board currentState)
        {
			Board nextState = currentState.copy();
			nextState.setTile(coord, tile);
			if (evaluateNeighboringCoordinates(nextState, coord))
			{
				return nextState;
			}
			else
            {
				return null;
            }
		}

		public static ISet<Board> getBiomeConfigurationsRecursive(Stack<HexagonalCoordinate> coordinates, TileCounts tileCounts, Board currentState)
        {
			if (coordinates.Count == 0)
            {
                Console.WriteLine("Found One!");
				return new HashSet<Board> { currentState };
            }

			ISet<Board> toReturn = new HashSet<Board>();
			HexagonalCoordinate nextCoordinate = coordinates.Pop();
			if (tileCounts.clay > 0)
            {
				Board? nextState = getNextValidState(Tile.CLAY, nextCoordinate, currentState);
				if (nextState != null)
                {
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeClay(), nextState));
                }
            }
			if (tileCounts.wood > 0)
			{
				Board? nextState = getNextValidState(Tile.WOOD, nextCoordinate, currentState);
				if (nextState != null)
				{
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeWood(), nextState));
				}
			}
			if (tileCounts.wheat > 0)
			{
				Board? nextState = getNextValidState(Tile.WHEAT, nextCoordinate, currentState);
				if (nextState != null)
				{
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeWheat(), nextState));
				}
			}
			if (tileCounts.sheep > 0)
			{
				Board? nextState = getNextValidState(Tile.SHEEP, nextCoordinate, currentState);
				if (nextState != null)
				{
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeSheep(), nextState));
				}
			}
			if (tileCounts.stone > 0)
			{
				Board? nextState = getNextValidState(Tile.STONE, nextCoordinate, currentState);
				if (nextState != null)
				{
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeStone(), nextState));
				}
			}
			if (tileCounts.desert > 0)
			{
				Board? nextState = getNextValidState(Tile.DESERT, nextCoordinate, currentState);
				if (nextState != null)
				{
					toReturn.UnionWith(getBiomeConfigurationsRecursive(new Stack<HexagonalCoordinate>(coordinates), tileCounts.removeDesert(), nextState));
				}
			}

			return toReturn;
		}
	}

	public class TileCounts
    {
		public int clay { get; protected set; }
		public int wood { get; protected set; }
		public int stone { get; protected set; }
		public int wheat { get; protected set; }
		public int sheep { get; protected set; }
		public int desert { get; protected set; }


		public TileCounts(int _clay, int _wood, int _stone, int _wheat, int _sheep, int _desert)
        {
			clay = _clay;
			wood = _wood;
			stone = _stone;
			wheat = _wheat;
			sheep = _sheep;
			desert = _desert;
        }

		public TileCounts removeClay()
        {
			return new TileCounts(clay - 1, wood, stone, wheat, sheep, desert);
        }

		public TileCounts removeWood()
		{
			return new TileCounts(clay, wood - 1, stone, wheat, sheep, desert);
		}

		public TileCounts removeStone()
		{
			return new TileCounts(clay, wood, stone - 1, wheat, sheep, desert);
		}

		public TileCounts removeWheat()
		{
			return new TileCounts(clay, wood, stone, wheat - 1, sheep, desert);
		}

		public TileCounts removeSheep()
		{
			return new TileCounts(clay, wood, stone, wheat, sheep - 1, desert);
		}

		public TileCounts removeDesert()
		{
			return new TileCounts(clay, wood, stone, wheat, sheep, desert - 1);
		}
	}
}

