using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minesweeper;

namespace minesweeper
{
    class GameLogic
    {
        // TODO: Move width, height and mine validation
        // from MineField constructor

        // instantiate the MineField
        private MineField game;

        //GameLogic constractor
        public GameLogic(int x, int y, int numMine)
        {
            //creating the minefield
             game =  new MineField(x,y,numMine);
        }

        public string ButtonLeftClicked(string btnName)
        {
            int cellLocationX;
            int cellLocationY;
            int.TryParse(btnName.Substring(3, 1), out cellLocationX);
            int.TryParse(btnName.Substring(4, 1), out cellLocationY);
            Cell chosenCell = new Cell();
            chosenCell = game.Cells[cellLocationX, cellLocationY];
            if (chosenCell.CellValue == 0)
            {
                //run spread function
            }
            else if (chosenCell.CellValue == 9)
            {
                //game over
            }

            return chosenCell.CellDisplayValue;
        }

    }


}
