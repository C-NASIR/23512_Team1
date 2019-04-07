using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minesweeper;

namespace Minesweeper.Classes
{
    class GameLogic
    {
        // TODO: Move game.Height, game.Width and mine validation
        // from MineField constructor

        // instantiate the MineField
        private MineField game;

        //string for sound filepath
        string lossPath = Environment.CurrentDirectory + "\\sounds" + "\\boom.wav";
        string victoryPath = Environment.CurrentDirectory + "\\sounds" + "\\victory.wav";


        //boolean for game over. True = win False = loss
        public bool gameEnd;

        //int for tracking scores
        private int current_score = 0;

        //set max score to number of bombs in play
        private int max_score;

        //set max number of flags player may use
        private int maxFlags;

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
            max_score = numMine;
            maxFlags = numMine + 2;
            //creating the minefield
            game = new MineField(x, y, numMine);
        }

        // On Right-clock, parses button name, read flagged status of cell in array at specified location,
        // toggles flag status in cell, returns "F" if flagged, "" if un-flagged
        // Tracks flags placed and stops user from using more than max
        // If all bombs are flagged play victory sound and display score
        public string ButtonRightClicked(string btnName)
        {
            string displayValue = "";
            int cellLocationX;
            int cellLocationY;
            string[] name = btnName.Split('_');
            int.TryParse(name[1], out cellLocationY);
            int.TryParse(name[2], out cellLocationX);
            Cell chosenCell = game.Cells[cellLocationY, cellLocationX];

            if (chosenCell.Flagged == false && flagCounter == maxFlags)
            {
                MessageBox.Show("Maximum number of flags placed.");
            }
            else if (chosenCell.Flagged == false && chosenCell.CellValue == 9)
            {
                current_score = current_score + 1;
                chosenCell.Flagged = true;
                FlagCounter++;
                displayValue = "F";
                if (current_score == max_score)
                {
                    MessageBox.Show("Victory! The final score was: " + (current_score * 10).ToString());

                    //load and play victory sound
                    //System.Media.SoundPlayer victoryPlayer = new System.Media.SoundPlayer();
                    //victoryPlayer.SoundLocation = victoryPath;
                    //victoryPlayer.Load();
                    //victoryPlayer.Play();
                }
            }
            else if (chosenCell.Flagged == false)
            {
                chosenCell.Flagged = true;
                FlagCounter++;
                displayValue = "F";
            }
            else if (chosenCell.Flagged == true && chosenCell.CellValue == 9)
            {
                chosenCell.Flagged = false;
                FlagCounter--;
                current_score--;
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
            int.TryParse(name[1], out cellLocationY);
            int.TryParse(name[2], out cellLocationX);
            Cell chosenCell = game.Cells[cellLocationY, cellLocationX];
            if (chosenCell.CellValue == 0)
            {
                chosenCell.Tagged = true;

                //run spread function
                stringCells = Spread(chosenCell);
                stringCells.Add(chosenCell.Column + "_" + chosenCell.Row.ToString());
            }
            else if (chosenCell.CellValue == 9)
            {
                //game over
                stringCells.Add(chosenCell.Column + "_" + chosenCell.Row.ToString());

                ////load and play explosion sound
                //System.Media.SoundPlayer boomPlayer = new System.Media.SoundPlayer();
                //boomPlayer.SoundLocation = lossPath;
                //boomPlayer.Load();
                //boomPlayer.Play();

                //message box for game over, set with MessageBoxResult for later use
                MessageBoxResult gameOverMessage = MessageBox.Show("Game Over");

                //close grid and show start menu again, right now simply closes application and reopens
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

                //set game to lost
                gameEnd = false;
            }
            else
            {
                stringCells.Add(chosenCell.Column + "_" + chosenCell.Row.ToString());
            }

            return stringCells;
        }

        // Takes clicked cell location and recursively searches for first cell with a value other than 0, 
        // continues search until hits an edge of the board or a value other than 0
        public List<string> Spread(Cell cell)
        {
            //returned list of cells to be displayed
            List<string> returnedCells = new List<string>();

            if ((cell.Column - 1 >= 0 && cell.Row - 1 >= 0)
                && (game.Cells[cell.Column - 1, cell.Row - 1].Tagged == false))
            {
                if (game.Cells[cell.Column - 1, cell.Row - 1].CellValue == 0)
                {
                    game.Cells[cell.Column - 1, cell.Row - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column - 1, cell.Row - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column - 1, cell.Row - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column - 1, cell.Row - 1].Column +
                                      "_" + game.Cells[cell.Column - 1, cell.Row - 1].Row.ToString());
                }
            }
            if ((cell.Row - 1 >= 0)
                && (game.Cells[cell.Column, cell.Row - 1].Tagged == false))
            {
                if (game.Cells[cell.Column, cell.Row - 1].CellValue == 0)
                {
                    game.Cells[cell.Column, cell.Row - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column, cell.Row - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column, cell.Row - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column, cell.Row - 1].Column +
                                      "_" + game.Cells[cell.Column, cell.Row - 1].Row.ToString());
                }
            }
            if ((cell.Column + 1 < game.Height && cell.Row - 1 >= 0)
                && (game.Cells[cell.Column + 1, cell.Row - 1].Tagged == false))
            {
                if (game.Cells[cell.Column + 1, cell.Row - 1].CellValue == 0)
                {
                    game.Cells[cell.Column + 1, cell.Row - 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column + 1, cell.Row - 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column + 1, cell.Row - 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column + 1, cell.Row - 1].Column +
                                      "_" + game.Cells[cell.Column + 1, cell.Row - 1].Row.ToString());
                }
            }
            if ((cell.Column - 1 >= 0)
                && (game.Cells[cell.Column - 1, cell.Row].Tagged == false))
            {
                if (game.Cells[cell.Column - 1, cell.Row].CellValue == 0)
                {
                    game.Cells[cell.Column - 1, cell.Row].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column - 1, cell.Row]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column - 1, cell.Row].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column - 1, cell.Row].Column +
                                      "_" + game.Cells[cell.Column - 1, cell.Row].Row.ToString());
                }
            }
            if ((cell.Column + 1 < game.Height)
                && (game.Cells[cell.Column + 1, cell.Row].Tagged == false))
            {
                if (game.Cells[cell.Column + 1, cell.Row].CellValue == 0)
                {
                    game.Cells[cell.Column + 1, cell.Row].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column + 1, cell.Row]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column + 1, cell.Row].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column + 1, cell.Row].Column +
                                      "_" + game.Cells[cell.Column + 1, cell.Row].Row.ToString());
                }
            }
            if ((cell.Column - 1 >= 0 && cell.Row + 1 < game.Width)
                && (game.Cells[cell.Column - 1, cell.Row + 1].Tagged == false))
            {
                if (game.Cells[cell.Column - 1, cell.Row + 1].CellValue == 0)
                {
                    game.Cells[cell.Column - 1, cell.Row + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column - 1, cell.Row + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column - 1, cell.Row + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column - 1, cell.Row + 1].Column +
                                      "_" + game.Cells[cell.Column - 1, cell.Row + 1].Row.ToString());
                }
            }
            if ((cell.Row + 1 < game.Width)
                && (game.Cells[cell.Column, cell.Row + 1].Tagged == false))
            {
                if (game.Cells[cell.Column, cell.Row + 1].CellValue == 0)
                {
                    game.Cells[cell.Column, cell.Row + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column, cell.Row + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column, cell.Row + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column, cell.Row + 1].Column +
                                      "_" + game.Cells[cell.Column, cell.Row + 1].Row.ToString());
                }
            }
            if ((cell.Column + 1 < game.Height && cell.Row + 1 < game.Width)
                && (game.Cells[cell.Column + 1, cell.Row + 1].Tagged == false))
            {
                if (game.Cells[cell.Column + 1, cell.Row + 1].CellValue == 0)
                {
                    game.Cells[cell.Column + 1, cell.Row + 1].Tagged = true;
                    returnedCells = returnedCells.Union(Spread(game.Cells[cell.Column + 1, cell.Row + 1]))
                         .OrderBy(x => x)
                         .ToList();
                }
                if (game.Cells[cell.Column + 1, cell.Row + 1].CellValue != 9)
                {
                    returnedCells.Add(game.Cells[cell.Column + 1, cell.Row + 1].Column +
                                      "_" + game.Cells[cell.Column + 1, cell.Row + 1].Row.ToString());
                }
            }
            return returnedCells;
        }
    }
}
