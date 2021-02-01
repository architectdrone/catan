using System;
using System.Collections.Generic;
namespace Catan
{
    public class Board
    {
        public const int SIZE = 2;
        Tile?[] boardState;
        bool[] isTileOkay; //indicates whether the tile is proven to be okay. True mean yes, False mean IDK.
        public Board()
        {
            int numberOfTiles = tilesInBoard(SIZE);
            boardState = new Tile?[numberOfTiles];
            isTileOkay = new bool[numberOfTiles];
            for (int i = 0; i < numberOfTiles; i++)
            {
                boardState[i] = null;
                isTileOkay[i] = false;
            }
        }

        public Board(Board oldBoard)
        {
            int numberOfTiles = tilesInBoard(SIZE);
            boardState = new Tile?[numberOfTiles];
            isTileOkay = new bool[numberOfTiles];
            for (int i = 0; i < numberOfTiles; i++)
            {
                boardState[i] = oldBoard.boardState[i];
                isTileOkay[i] = oldBoard.isTileOkay[i];
            }
        }

        /**
        * Returns the number of tiles in a board of the given size.
        */
        public static int tilesInBoard(int size)
        {
            //Each ring on the board has six more tiles than the one before it. So,
            //Ring 0 = 1
            //Ring 1 = 6
            //Ring 2 = Ring1 + 6 = 12
            //Ring 3 = Ring2 + 6 = 18
            //etc.
            //Therefore, the number of tiles on a given ring is given by 6*x (x != 0), where x is the ring number
            //Using this, we can calculate the number of tiles in a board of a given size:
            //Board 0 = 1 = 1
            //Board 1 = 6*1 + 1 = 7
            //Board 2 = 6*2 + 6*1 + 1 = 19
            //Board 3 = 6*3 + 6*2 + 6*1 + 1 = ...
            //With a little bit of algebra we arrive at (6*(((n)(n+1))/2))+1, where "n" is the size of the board.

            if (size == 0)
            {
                return 1;
            }
            else
            {
                return 6 * (((size) * (size + 1)) / 2) + 1;
            }
        }

        /**
        * Returns the index that the hexagonal coordinate corresponds to.
        */
        public static int getIndex(HexagonalCoordinate coordinate)
        {
            /*
            PURPOSE:
            This algorithim maps a hexagonal coordinate to a corresponding index.
            
            OVERVIEW:
            The array is split into sections representing each of the rings. Each of the rings is mapped onto the array "clockwise" from a given axis.
            It doesn't really matter which direction you go around in, or what axis is your starting point.
            I am using +z as the starting point.

            ALGORITHIM:
            
            -- Determining what ring the tile is on
            This is pretty simple. Take absolute value of each of the three axises. The greatest of the three is the ring number.
            For example, (1, 1, -2) is on ring 2.
            
            -- Distance to center axis
            
            ----Overview
            We will approach this by dividing the problem into two parts. 
            First, we will determine the distance from the tile to the nearest axis.
            Last, we will determine the distance from the nearest axis to the center axis.
            
            ----Note on axis names
            In order to distinguish between the axes, I have given them some names. 
            +z - (z = 0) and x > 0
            -z - (z = 0) and x < 0
            +y - (y = 0) and x > 0
            -y - (y = 0) and x < 0
            +x - (x = 0) and y > 0
            -x - (x = 0) and y < 0

            ----Distance to nearest axis
            The axises of the board split the board much like a pizza. What is left after removing is a slice. 
            We can determine which slice we are in by comparing the numbers in the coordinates. Specifically, we can look at the placement and sign of the ring number.
            The following were determined by experimentation and observation, but I don't have any reason to doubt that they are accurate.
            - If RN in X, and RN is + => Next axis is (+z), and distance to axis is abs(z).
            - If RN in X, and RN is - => Next axis is (-z), and distance to axis is abs(z).
            - If RN in Y, and RN is + => Next axis is (+x), and distance to axis is abs(x).
            - If RN in Y, and RN is - => Next axis is (-x), and distance to axis is abs(x).
            - If RN in Z, and RN is + => Next axis is (+y), and distance to axis is abs(y).
            - If RN in Z, and RN is - => Next axis is (-y), and distance to axis is abs(y).
            (Of course, one should first check that you aren't on an axis. If you are, then the distance to the next axis is 0 (duh))
            
            ----Distance from nearest axis to center axis
            The first thing to realize is that the distance between the axises is equal to (RingNumber-1).
            Next, we should establish how many axises are between the current axis and the center axis. I call this "axial distance"
            - +x is 2 away
            - -x is 5 away
            - +y is 1 away
            - -y is 4 away
            - +z is 0 away
            - -z is 3 away
            The distance from the nearest axis to the center axis is therefore AxialDistance*RingNumber
            */

            if (coordinate.isOrigin())
            {
                return 0;
            }

            int x = coordinate.x;
            int y = coordinate.y;
            int z = coordinate.z;

            int ringNumber = Math.Max(Math.Abs(x), Math.Max(Math.Abs(y), Math.Abs(z))); //Get the max of x, y, and z
            int ringOffset = tilesInBoard(ringNumber - 1);

            int axialDistance;
            int tilesToNextAxis;

            if (y == ringNumber && z == -1 * ringNumber)
            {
                //On the +x axis
                axialDistance = 2;
                tilesToNextAxis = 0;
            }
            else if (z == ringNumber && y == -1 * ringNumber)
            {
                //On the -x axis
                axialDistance = 5;
                tilesToNextAxis = 0;
            }
            else if (x == ringNumber && z == -1 * ringNumber)
            {
                //On the +y axis
                axialDistance = 1;
                tilesToNextAxis = 0;
            }
            else if (z == ringNumber && x == -1 * ringNumber)
            {
                //On the -y axis
                axialDistance = 4;
                tilesToNextAxis = 0;
            }
            else if (x == ringNumber && y == -1 * ringNumber)
            {
                //On the +z axis
                axialDistance = 0;
                tilesToNextAxis = 0;
            }
            else if (x == -1 * ringNumber && y == ringNumber)
            {
                //On the -z axis
                axialDistance = 3;
                tilesToNextAxis = 0;
            }
            else if (x == ringNumber)
            {
                //Next axis is +Z
                axialDistance = 0;
                tilesToNextAxis = Math.Abs(z);
            }
            else if (x == -1 * ringNumber)
            {
                //Next axis is -Z
                axialDistance = 3;
                tilesToNextAxis = Math.Abs(z);
            }
            else if (y == ringNumber)
            {
                //Next axis is +X
                axialDistance = 2;
                tilesToNextAxis = Math.Abs(x);
            }
            else if (y == -1 * ringNumber)
            {
                //Next axis is -X
                axialDistance = 5;
                tilesToNextAxis = Math.Abs(x);
            }
            else if (z == ringNumber)
            {
                //Next axis is +Y
                axialDistance = 4;
                tilesToNextAxis = Math.Abs(y);
            }
            else
            {
                //Next axis is -Y
                axialDistance = 1;
                tilesToNextAxis = Math.Abs(y);
            }
            int result = ringOffset + tilesToNextAxis + (axialDistance * ringNumber);
            return result;
        }

        /*
         * Returns the ring that the given index sits in.
         */
        public static int indexToRing(int index)
        {
            if (index == 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Floor((3 + Math.Sqrt((12 * index) - 3)) / 6);
            }
        }

        /**
         * Converts an index to a hexagonal coordinate. Yes, this is the inverse of getIndex
         */
        public static HexagonalCoordinate indexToCoordinate(int index)
        {
            if (index == 0)
            {
                return new HexagonalCoordinate(0, 0, 0);
            }

            //Get the ring that the index resides on.
            int ring = indexToRing(index);

            //Then, the number of tiles in the board before that ring.
            int tiles = tilesInBoard(ring - 1);
            int tilesInRing = 6 * ring; //briefly explained in the ring function
            int sliceSize = ring - 1; //Distance between axes. 

            //Using these two together, we can calculate the offset from the center axis.
            int ringOffset = index - tiles;

            //Handle the case where we are on an axis.
            if (ringOffset % (sliceSize+1) == 0)
            {
                switch (ringOffset / (sliceSize+1))
                {
                    case 0:return new HexagonalCoordinate(ring, -ring, 0);
                    case 1:return new HexagonalCoordinate(ring, 0, -ring);
                    case 2:return new HexagonalCoordinate(0, ring, -ring);
                    case 3:return new HexagonalCoordinate(-ring, ring, 0);
                    case 4:return new HexagonalCoordinate(-ring, 0, ring);
                    case 5:return new HexagonalCoordinate(0, -ring, ring);
                    default: throw new ArithmeticException();
                }
            }

            //Since we are not on an axis, we are on a slice. Use the observations from getIndex to complete...
            int pos_x = 0;
            int pos_y = 1 * (sliceSize + 1);
            int pos_z = 2 * (sliceSize + 1);
            int neg_x = 3 * (sliceSize + 1);
            int neg_y = 4 * (sliceSize + 1);
            int neg_z = 5 * (sliceSize + 1);

            int sliceOffset = ringOffset % (sliceSize + 1);
            int thirdCoordinate = ring - sliceOffset;

            if ((pos_x < ringOffset) && (ringOffset < pos_y))
            {
                //between +x and +y
                return new HexagonalCoordinate(ring, -(thirdCoordinate), -sliceOffset);
            }
            if ((pos_y < ringOffset) && (ringOffset < pos_z))
            {
                //between +y and +z
                return new HexagonalCoordinate((thirdCoordinate), sliceOffset, -ring);
            }
            if ((pos_z < ringOffset) && (ringOffset < neg_x))
            {
                //between +z and -x
                return new HexagonalCoordinate(-sliceOffset, ring, -(thirdCoordinate));
            }
            if ((neg_x < ringOffset) && (ringOffset < neg_y))
            {
                //between -x and -y
                return new HexagonalCoordinate(-ring, (thirdCoordinate), sliceOffset);
            }
            if ((neg_y < ringOffset) && (ringOffset < neg_z))
            {
                //between -y and -z
                return new HexagonalCoordinate(-(thirdCoordinate), -sliceOffset, ring);
            }
            else
            {
                //between -z and +x
                return new HexagonalCoordinate(sliceOffset, -ring, (thirdCoordinate));
            }
        }

        public static ISet<HexagonalCoordinate> getCoordinatesInRing(int ringNumber)
        {
            if (ringNumber == 0)
            {
                return new HashSet<HexagonalCoordinate> { new HexagonalCoordinate(0,0,0)};
            }
            HashSet<HexagonalCoordinate> toReturn = new HashSet<HexagonalCoordinate>();

            //Add the six axes
            toReturn.Add(new HexagonalCoordinate(ringNumber, 0, -1 * ringNumber));
            toReturn.Add(new HexagonalCoordinate(-1*ringNumber, 0, ringNumber));
            toReturn.Add(new HexagonalCoordinate(ringNumber, -1 * ringNumber, 0));
            toReturn.Add(new HexagonalCoordinate(-1 * ringNumber, ringNumber, 0));
            toReturn.Add(new HexagonalCoordinate(0, ringNumber, -1 * ringNumber));
            toReturn.Add(new HexagonalCoordinate(0, -1 * ringNumber, ringNumber));

            //Add elements from the six slices
            for (int a = 1; a <= ringNumber; a++)
            {
                //Other Coordinate
                int b = ringNumber - a;

                toReturn.Add(new HexagonalCoordinate(ringNumber, -1 * a, -1 * b));
                toReturn.Add(new HexagonalCoordinate(a, b, -1 * ringNumber));
                toReturn.Add(new HexagonalCoordinate(-1 * b, ringNumber, -1 * a));
                toReturn.Add(new HexagonalCoordinate(-1*ringNumber, a, b));
                toReturn.Add(new HexagonalCoordinate(-1*a, -1 * b, ringNumber));
                toReturn.Add(new HexagonalCoordinate(b, -1 * ringNumber, a));
            }

            return toReturn;
        }

        /*
         * Get the coordinates in a board of the given size.
         */
        public static ISet<HexagonalCoordinate> getCoordinatesInBoard(int size)
        {
            if (size == 0)
            {
                return getCoordinatesInRing(0);
            }
            else
            {
                ISet<HexagonalCoordinate> inBoard = getCoordinatesInBoard(size - 1);
                inBoard.UnionWith(getCoordinatesInRing(size));
                return inBoard;
            }
        }

        public void setTile(HexagonalCoordinate coord, Tile? tile)
        {
            if (!isOnBoard(coord))
            {
                throw new ArithmeticException();
            }
            boardState[getIndex(coord)] = tile;
        }

        public void markOkay(HexagonalCoordinate coord)
        {
            isTileOkay[getIndex(coord)] = true;
        }

        public bool isOkay(HexagonalCoordinate coord)
        {
            return isTileOkay[getIndex(coord)];
        }

        public Tile? getTile(HexagonalCoordinate coord)
        {
            if (!isOnBoard(coord))
            {
                throw new ArithmeticException();
            }
            return boardState[getIndex(coord)];
        }

        /**
        * Returns true if the given coordinates exist on the board.
        */
        public static bool isOnBoard(HexagonalCoordinate coordinate)
        {
            return (Math.Abs(coordinate.x) <= SIZE && Math.Abs(coordinate.y) <= SIZE && Math.Abs(coordinate.z) <= SIZE);
        }

        public void print()
        {
            char _1 = tileToChar(getTile(new HexagonalCoordinate(0, 2, -2)));
            char _2 = tileToChar(getTile(new HexagonalCoordinate(1,1, -2)));
            char _3 = tileToChar(getTile(new HexagonalCoordinate(2,0, -2)));
            char _4 = tileToChar(getTile(new HexagonalCoordinate(-1, 2, -1)));
            char _5 = tileToChar(getTile(new HexagonalCoordinate(0,1,-1)));
            char _6 = tileToChar(getTile(new HexagonalCoordinate(1,0,-1)));
            char _7 = tileToChar(getTile(new HexagonalCoordinate(2,-1,-1)));
            char _8 = tileToChar(getTile(new HexagonalCoordinate(-2,2,0)));
            char _9 = tileToChar(getTile(new HexagonalCoordinate(-1,1,0)));
            char _10 = tileToChar(getTile(new HexagonalCoordinate(0,0,0)));
            char _11 = tileToChar(getTile(new HexagonalCoordinate(1, -1, 0)));
            char _12 = tileToChar(getTile(new HexagonalCoordinate(2, -2, 0)));
            char _13 = tileToChar(getTile(new HexagonalCoordinate(-2, 1, 1)));
            char _14 = tileToChar(getTile(new HexagonalCoordinate(-1, 1, 0)));
            char _15 = tileToChar(getTile(new HexagonalCoordinate(0, -1, 1)));
            char _16 = tileToChar(getTile(new HexagonalCoordinate(1, -2, 1)));
            char _17 = tileToChar(getTile(new HexagonalCoordinate(-2, 0, 2)));
            char _18 = tileToChar(getTile(new HexagonalCoordinate(-1, -1, 2)));
            char _19 = tileToChar(getTile(new HexagonalCoordinate(0, -2, 2)));

            Console.WriteLine($"  {_1} {_2} {_3}");
            Console.WriteLine($" {_4} {_5} {_6} {_7}");
            Console.WriteLine($"{_8} {_9} {_10} {_11} {_12}");
            Console.WriteLine($" {_13} {_14} {_15} {_16}");
            Console.WriteLine($"  {_17} {_18} {_19}");
        }

        private static char tileToChar(Tile? tile)
        {
            if (tile == null) return '?';
            else if (tile == Tile.CLAY) return 'c';
            else if (tile == Tile.WOOD) return 'l';
            else if (tile == Tile.SHEEP) return 'w';
            else if (tile == Tile.STONE) return 'o';
            else if (tile == Tile.WHEAT) return 'g';
            else return 'd';
        }
    }

    public enum Tile
    {
        WOOD,
        STONE,
        SHEEP,
        WHEAT,
        CLAY,
        DESERT
    }
}