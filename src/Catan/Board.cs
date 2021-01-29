using System;
using System.Collections.Generic;
namespace Catan
{
    public class Board
    {
        const int SIZE = 2;
        Tile[] boardState;
        public Board()
        {


            int numberOfTiles = tilesInBoard(SIZE);
            boardState = new Tile[numberOfTiles];
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
            //Board 1 = 6*1 + 1 = 6 + 1 = 7
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

        /**
        * Returns true if the given coordinates exist on the board.
        */
        public bool isOnBoard(HexagonalCoordinate coordinate)
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