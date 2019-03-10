using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace minesweeper
{
    class GameLogic : MainWindow
    {
        // TODO: Move width, height and mine validation
        // from MineField constructor

        // instantiate the MineField
        MineField game = new MineField(9, 9, 3);

        //GameLogic constractor
        public GameLogic(string x, string y, string numMine)
        {
            MineField game = new MineField(x, y, numMine);
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


        //Check the User input 
        public bool Inputchecker(string xNumber, string yNumber, string numMines)
        {
            bool checker =  false;
            int x, y, numMine;
            //checking if the user entered legit number of columns
            if (int.TryParse(xNumber, out x))
            {
                //checking if the user entered legit number of rows
                if (int.TryParse(yNumber, out y))
                {
                    //Calling the dynamic grid creator method
                    DynamicGridCreator(x, y);
                    checker = true;
                }
                else
                {
                    //catching the error if the user enteres invalid number rows
                    MessageBox.Show("Please Enter a Valid number of rows");
                }
            }
            else
            {
                //catching the error if the user enteres invalid number of columns
                MessageBox.Show("Please Enter a valid number of columns");
            }

            return checker;
        }


    }


}
