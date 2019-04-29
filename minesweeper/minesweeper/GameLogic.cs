using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minesweeper;
using System.Windows.Input;

namespace minesweeper
{
    class GameLogic
    {
        // TODO: Move game.Height, game.Width and mine validation
        // from MineField constructor

        // instantiate the MineField
        private MineField game;

        //string for sound filepath
        string lossPath = Environment.CurrentDirectory + "\\.." + "\\.." + "\\sounds" + "\\boom.wav";
        string victoryPath = Environment.CurrentDirectory + "\\.." +"\\.." + "\\sounds" + "\\victory.wav";

        //int for tracking scores
        private int current_score = 0;

        //set max score to number of bombs in play
        private int max_score = int.Parse(((MainWindow)Application.Current.MainWindow).txtBombs.Text);

        //set max number of flags player may use
        private int maxFlags = int.Parse(((MainWindow)Application.Current.MainWindow).txtBombs.Text) + 2;

        private int flagCounter = 0;

        //used to determine if the player clicked on a bomb
        private bool gameEnd = false;

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

        public bool GameEnd
        {
            get { return gameEnd; }
        }

        //GameLogic constructor
        public GameLogic(int y, int x, int numMine)
        {
            //creating the minefield
             game =  new MineField(x,y,numMine);
        }

        // On Right-clock, parses button name, read flagged status of cell in array at specified location,
        // toggles flag status in cell, returns the status of the flag
        // Tracks flags placed and stops user from using more than max
        public bool ButtonRightClicked(string btnName)
        {
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
            else if (chosenCell.Flagged == false)
            {
                chosenCell.Flagged = true;
                FlagCounter++;
            }
            else if (chosenCell.Flagged == true)
            {
                chosenCell.Flagged = false;
                FlagCounter--;
            }
            return chosenCell.Flagged;
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
                stringCells.Add(chosenCell.YLocation + "_" + chosenCell.XLocation.ToString());
            }
            else if (chosenCell.CellValue == 9)
            {
                //game over
                stringCells.Add(chosenCell.YLocation + "_" + chosenCell.XLocation.ToString());

                //load and play explosion sound
                System.Media.SoundPlayer boomPlayer = new System.Media.SoundPlayer();
                boomPlayer.SoundLocation = lossPath;
                boomPlayer.Load();
                boomPlayer.Play();

                //message box for game over, set with MessageBoxResult for later use
                MessageBoxResult gameOverMessage = MessageBox.Show("Game Over");

                gameEnd = true;

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

        /// <summary>
        /// Returns the calculated score based on the time, and flag positions
        /// </summary>
        /// <returns></returns>
        internal int CalculateScore(String time)
        {
            int score = 0;
            int hours;
            int min;
            int sec;

            //parses the time it took the player to play the game and 
            //adjust the score accordingly
            string[] timeScore = time.Split(':');
            int.TryParse(timeScore[0], out hours);
            score += hours * 3600;
            int.TryParse(timeScore[1], out min);
            score += min * 60;
            int.TryParse(timeScore[2], out sec);
            score += sec;

            //determines where flags and mines are placed and if the player
            //correctly flagged mines, missed mines, or flagged incorrect cells
            //and adjusts the score accordingly
            foreach (Cell c in Game.Cells)
            {
                if (c.Flagged && c.CellValue == 9)
                {
                    score -= 15;
                }
                else if (c.Flagged && c.CellValue != 9)

                {
                    score += 25;
                }
                else if (c.Flagged == false && c.CellValue == 9)
                {
                    score += 25;
                }
            }

            return score;
        }
    }
}
