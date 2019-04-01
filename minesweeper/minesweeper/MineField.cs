using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        
        // public access to width
        public int Width
        {
            get { return width; }
        }

        // public access to height
        public int Height
        {
            get { return height; }
        }

        // public access to cells
        public Cell[,] Cells
        {
            get { return cells; }
        }

        // MineField Constructor
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
                    cells = new Cell[height, width];

                    // instantiate the remaining cells (Joe)
                    PopulateCells();

                    // instantiate the mine cells and add to the array
                    for (int x = 0; x < mines; x++)
                    {
                        CreateMines();
                    }

                    // sets the cell values to the appropriate numbers based on the mine placements
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (cells[y, x].CellValue != 9)
                            {
                                cells[y, x].CellValue = CreateValue(cells[y, x]);
                            }                        
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("You must have at least 1 mine");
                }
            }

            else
            {
                MessageBox.Show("The number of mines can make up no more than \n one third of the available cells.");
            }
        }

        // Instantiate all minefield cells (Joe)
        private void PopulateCells()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[y, x] = new Cell(0,x,y);
                }
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
                if (cells[y, x].CellValue != 9)
                {
                    cells[y, x].CellValue = 9;
                    sameLocation = false;
                }
                else
                {
                    sameLocation = true;
                }
            } while (sameLocation);
        }

        // checks all positions around a cell and returns an int that is the number of mines around the cell
        private int CreateValue(Cell cell)
        {
            int value = 0;
            if (cell.YLocation - 1 >= 0 && cell.XLocation - 1 >= 0)
            {
                if (cells[cell.YLocation - 1, cell.XLocation - 1].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.XLocation - 1 >= 0)
            {
                if (cells[cell.YLocation, cell.XLocation - 1].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.YLocation + 1 < height && cell.XLocation - 1 >= 0)
            {
                if (cells[cell.YLocation + 1, cell.XLocation - 1].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.YLocation - 1 >= 0)
            {
                if (cells[cell.YLocation - 1, cell.XLocation].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.YLocation + 1 < height)
            {
                if (cells[cell.YLocation + 1, cell.XLocation].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.YLocation - 1 >= 0 && cell.XLocation + 1 < width)
            {
                if (cells[cell.YLocation - 1, cell.XLocation + 1].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.XLocation + 1 < width)
            {
                if (cells[cell.YLocation, cell.XLocation + 1].CellValue == 9)
                {
                    value++;
                }
            }
            if (cell.YLocation + 1 < height && cell.XLocation + 1 < width)
            {
                if (cells[cell.YLocation + 1, cell.XLocation + 1].CellValue == 9)
                {
                    value++;
                }
            }
            return value;
        }
    }
}
