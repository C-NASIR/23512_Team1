using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class MineField
    {
        // Keeps Track of the number of cells across the grid' (x-locations)
        private static int width;

        // Keeps Track of the number of cells down the grid (y-locations)
        private static int height;

        // Keeps track of the number of mines in the grid
        private static int mines;

        // The Grid array
        private Cell[,] cells;

        // The timer
        // TODO: Create Timer

        // public access to cells
        public Cell[,] Cells
        {
            get { return cells; }
        }

        // Grid Constructor
        public MineField(int xWidth, int yHeight, int numMines)
        {
            // set the width and height, and create the array
            if ((xWidth * yHeight) > (numMines * 3))
            {
                if (numMines > 0)
                {
                    mines = numMines;
                    width = xWidth;
                    height = yHeight;
                    cells = new Cell[width, height];

                    // instantiate the mine cells and add to the array
                    for (int x = 0; x < mines; x++)
                    {
                        CreateMines();
                    }

                    // instantiate the remaining cells
                }
                else
                {
                    //MessageBox.Show("You must have at least 1 mine");
                }
            }

            else
            {
                //MessageBox.Show("The number of mines can make up no more than \n one third of the available cells.");
            }
        }

        // Cell Coordinate generator method, pass in grid width or height
        private int MineCoordinateGenerator(int gridLength)
        {
            int coordinate;
            Random coord = new Random();
            coordinate = coord.Next(1, gridLength - 1);
            return coordinate;
        }

        // set mine coordinates
        private void CreateMines()
        {
            bool sameLocation;
            int x;
            int y;
            do
            {
                x = MineCoordinateGenerator(width);
                y = MineCoordinateGenerator(height);
                if (cells[x, y] == null)
                {
                    cells[x, y] = new Cell(9, x, y);
                    sameLocation = false;
                }
                else
                {
                    sameLocation = true;
                }
            } while (sameLocation);
        }
    }
}
