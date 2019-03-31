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

        private int flagCounter = 0;

        // public property of minefield
        public MineField Game
        {
            get { return game; }
        }

        public int FlagCounter
        {
            get { return flagCounter; }
            set { flagCounter = value; }
        }

        //GameLogic constructor
        public GameLogic(int y, int x, int numMine)
        {
            //creating the minefield
             game =  new MineField(x,y,numMine);
        }

        // On Right-clock, parses button name, read flagged status of cell in array at specified location,
        // toggles flag status in cell, returns "F" if flagged, "" if un-flagged
        public string ButtonRightClicked(string btnName)
        {
            string displayValue = "";
            int cellLocationX;
            int cellLocationY;
            string[] name = btnName.Split('_');
            int.TryParse(name[1], out cellLocationY);   //switch this y and x where we parse
            int.TryParse(name[2], out cellLocationX);
            Cell chosenCell = game.Cells[cellLocationY, cellLocationX];

            if (chosenCell.Flagged == false)
            {
                chosenCell.Flagged = true;
                FlagCounter++;
                displayValue = "F";
            }
            else
            {
                chosenCell.Flagged = false;
                FlagCounter--;
            }

            return displayValue;
        }

        // On Left-click, parses button name, read cell value in array at specified location
        // calls spread function as necessary and returns a string of cell locations to affect on the UI
        public List<string> ButtonLeftClicked(string btnName)
        {

            List<string> stringCells = new List<string>();

            int cellLocationX;
            int cellLocationY;
            string[] name = btnName.Split('_');
            int.TryParse(name[1], out cellLocationY);   //switch this y and x where we parse
            int.TryParse(name[2], out cellLocationX);
            Cell chosenCell = game.Cells[cellLocationY, cellLocationX];
            if (chosenCell.CellValue == 0)
            {
                chosenCell.Tagged = true;
                
                //run spread function
                stringCells = Spread(chosenCell);
                stringCells.Add(chosenCell.YLocation + "_" + chosenCell.XLocation.ToString());
            }
            else if (chosenCell.CellValue == 9)
            {
                //game over
                stringCells.Add(chosenCell.YLocation + "_" + chosenCell.XLocation.ToString());
            }
            else
            {
                stringCells.Add(chosenCell.YLocation + "_" + chosenCell.XLocation.ToString());
            }

            return stringCells;
        }

        // Takes clicked cell location and recursively searches for first cell with a value other than 0, 
        // continues search until hits an edge of the board or a value other than 0
        public List<string> Spread(Cell cell)
        {
            //returned list of cells to be displayed
            List<string> returnedCells = new List<string>();

            if ((cell.YLocation - 1 >= 0 && cell.XLocation - 1 >= 0)
                &&(game.Cells[cell.YLocation - 1, cell.XLocation - 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if(game.Cells[cell.YLocation - 1, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation - 1].YLocation +
                                      "_" + game.Cells[cell.YLocation - 1, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if ((cell.XLocation - 1 >= 0)
                &&(game.Cells[cell.YLocation, cell.XLocation - 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation, cell.XLocation - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation, cell.XLocation - 1].YLocation +
                                      "_" + game.Cells[cell.YLocation, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if ((cell.YLocation + 1 < game.Height && cell.XLocation - 1 >= 0)
                &&(game.Cells[cell.YLocation + 1, cell.XLocation - 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation - 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation + 1, cell.XLocation - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation - 1].YLocation +
                                      "_" + game.Cells[cell.YLocation + 1, cell.XLocation - 1].XLocation.ToString());
                }
            }
            if ((cell.YLocation - 1 >= 0)
                &&(game.Cells[cell.YLocation - 1, cell.XLocation].Tagged == false))
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation - 1, cell.XLocation].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation].YLocation +
                                      "_" + game.Cells[cell.YLocation - 1, cell.XLocation].XLocation.ToString());
                }
            }
            if ((cell.YLocation + 1 < game.Height)
                &&(game.Cells[cell.YLocation + 1, cell.XLocation].Tagged == false))
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation + 1, cell.XLocation].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation].YLocation +
                                      "_" + game.Cells[cell.YLocation + 1, cell.XLocation].XLocation.ToString());
                }
            }
            if ((cell.YLocation - 1 >= 0 && cell.XLocation + 1 < game.Width)
                &&(game.Cells[cell.YLocation - 1, cell.XLocation + 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation - 1, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation - 1, cell.XLocation + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation - 1, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation - 1, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation - 1, cell.XLocation + 1].YLocation +
                                      "_" + game.Cells[cell.YLocation - 1, cell.XLocation + 1].XLocation.ToString());
                }
            }
            if ((cell.XLocation + 1 < game.Width)
                &&(game.Cells[cell.YLocation, cell.XLocation + 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation, cell.XLocation + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation, cell.XLocation + 1].YLocation +
                                      "_" + game.Cells[cell.YLocation, cell.XLocation + 1].XLocation.ToString());
                }
            }
            if ((cell.YLocation + 1 < game.Height && cell.XLocation + 1 < game.Width)
                &&(game.Cells[cell.YLocation + 1, cell.XLocation + 1].Tagged == false))
            {
                if (game.Cells[cell.YLocation + 1, cell.XLocation + 1].CellValue == 0)
                {
                    game.Cells[cell.YLocation + 1, cell.XLocation + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.YLocation + 1, cell.XLocation + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.YLocation + 1, cell.XLocation + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.YLocation + 1, cell.XLocation + 1].YLocation +
                                      "_" + game.Cells[cell.YLocation + 1, cell.XLocation + 1].XLocation.ToString());
                }
            }
            return returnedCells;
        }
    }
}
