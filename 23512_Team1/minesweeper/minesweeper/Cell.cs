using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class Cell
    {
        // the location x coordinate
        private int xLocation;

        // the location y coordinate
        private int yLocation;

        // whether the cell is flagged
        private bool flagged;

        // the cell value (0 - 9, 9 = mine)
        private int cellValue;

        // text display value
        private string cellDisplayValue;

        // Cell Constructor 
        public Cell(int pCellValue, int pXLocation, int pYLocation)
        {
            cellValue = pCellValue;
            xLocation = pXLocation;
            yLocation = pYLocation;
            cellDisplayValue = CellDisplayValue;
            flagged = false;
        }

        // public property of xLocation
        public int XLocation
        {
            get { return xLocation; }
        }

        // public property of yLocation
        public int YLocation
        {
            get { return yLocation; }
        }

        // public property of flagged
        public bool Flagged
        {
            get { return flagged; }
            set { flagged = value; }
        }

        // public property of cell value
        public int CellValue
        {
            get { return cellValue; }
        }

        // public property of cell display value
        public string CellDisplayValue
        {
            get
            {
                if (cellValue < 9)
                {
                    if (cellValue != 0)
                    {
                        cellDisplayValue = cellValue.ToString();
                    }
                    else
                    {
                        cellDisplayValue = " ";
                    }
                }
                else
                {
                    cellDisplayValue = "*";
                }
                return cellDisplayValue;
            }
        }
    }
}

