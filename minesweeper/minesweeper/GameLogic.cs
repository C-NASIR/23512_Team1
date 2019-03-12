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
        // TODO: Move game.Height, game.Width and mine validation
        // from MineField constructor

        // instantiate the MineField
        private MineField game;

        // public property of minefield
        public MineField Game
        {
            get { return game; }
        }

        //GameLogic constractor
        public GameLogic(int x, int y, int numMine)
        {
            //creating the minefield
             game =  new MineField(x,y,numMine);
        }

        public List<string> ButtonLeftClicked(string btnName)
        {

            List<string> stringCells = new List<string>();

            int cellLocationX;
            int cellLocationY;
            int.TryParse(btnName.Substring(3, 1), out cellLocationY);   //switch this y and x where we parse
            int.TryParse(btnName.Substring(4, 1), out cellLocationX);
            Cell chosenCell = game.Cells[cellLocationY, cellLocationX];
            if (chosenCell.CellValue == 0)
            {
                chosenCell.CellValue = -1;
                //run spread function
                stringCells = Spread(chosenCell);

            }
            else if (chosenCell.CellValue == 9)
            {
                //game over
                stringCells.Add(chosenCell.YLocation + chosenCell.XLocation.ToString());
            }
            else
            {
                stringCells.Add(chosenCell.YLocation + chosenCell.XLocation.ToString());
            }

            return stringCells;
        }

        public List<string> Spread(Cell cell)
        {
            //returned list of cells to be displayed
            List<string> returnedCells = new List<string>();

            if (cell.YLocation - 1 >= 0 && cell.XLocation - 1 >= 0)
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation - 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if(game.Cells[cell.YLocation - 1, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation - 1].YLocation +
                        game.Cells[cell.YLocation - 1, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if (cell.XLocation - 1 >= 0)
            {
                if (game.Cells[cell.YLocation, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation, cell.XLocation - 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation, cell.XLocation - 1].YLocation +
                        game.Cells[cell.YLocation, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if (cell.YLocation + 1 < game.Height && cell.XLocation - 1 >= 0)
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation - 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation + 1, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation - 1].YLocation +
                        game.Cells[cell.YLocation + 1, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if (cell.YLocation - 1 >= 0)
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation - 1, cell.XLocation].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation].YLocation +
                        game.Cells[cell.YLocation - 1, cell.XLocation].XLocation.ToString());
                }
            }
            if (cell.YLocation + 1 < game.Height)
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation + 1, cell.XLocation].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation].YLocation +
                        game.Cells[cell.YLocation + 1, cell.XLocation].XLocation.ToString());
                }
            }
            if (cell.YLocation - 1 >= 0 && cell.XLocation + 1 < game.Width)
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation + 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation - 1, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation + 1].YLocation +
                        game.Cells[cell.YLocation - 1, cell.XLocation + 1].XLocation.ToString());
                }
            }
            if (cell.XLocation + 1 < game.Width)
            {
                if (game.Cells[cell.YLocation, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation, cell.XLocation + 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation, cell.XLocation + 1].YLocation +
                        game.Cells[cell.YLocation, cell.XLocation + 1].XLocation.ToString());
                }
            }
            if (cell.YLocation + 1 < game.Height && cell.XLocation + 1 < game.Width)
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation + 1].CellValue = -1;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                else if (game.Cells[cell.YLocation + 1, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation + 1].YLocation +
                        game.Cells[cell.YLocation + 1, cell.XLocation + 1].XLocation.ToString());
                }
            }
            return returnedCells;
        }
    }
}
