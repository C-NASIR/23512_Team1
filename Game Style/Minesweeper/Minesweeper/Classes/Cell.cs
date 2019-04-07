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
        private int column;

        // the location y coordinate
        private int row;

        // check if it is flagged
        private bool flagged;

        //check if it is already checked
        private bool tagged;

        // the cell value (0 - 9, 9 = mine)
        private int cellValue;

        // text display value
        private string cellDisplayValue;

        // Cell Constructor 
        public Cell(int column, int row, int cellValue)
        {
            this.cellValue = cellValue;
            this.column = column;
            this.row = row;
        }

        // public property of xLocation
        public int Column
        {
            get { return column; }
        }

        // public property of yLocation
        public int Row
        {
            get { return row; }
        }

        // public property of tagged
        public bool Tagged
        {
            get { return tagged; }
            set { tagged = value; }
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
            set { cellValue = value; }
        }

        // public property of cell display value
        public string CellDisplayValue
        {
            get
            {
                if (cellValue < 9)
                {
                    if (cellValue > 0)
                    {
                        cellDisplayValue = cellValue.ToString();
                    }
                    else
                    {
                        cellDisplayValue = "0";
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
