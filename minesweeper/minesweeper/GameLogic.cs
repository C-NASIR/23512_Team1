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
        string lossPath = Environment.CurrentDirectory + "\\sounds" + "\\boom.wav";
        string victoryPath = Environment.CurrentDirectory + "\\sounds" + "\\victory.wav";


        //boolean for game over. True = win False = loss
        public bool gameEnd;

        //int for tracking scores
        private int current_score = 0;

        //set max score to number of bombs in play
        private int max_score = int.Parse(((MainWindow)Application.Current.MainWindow).txtBombs.Text);

        //set max number of flags player may use
        private int maxFlags = int.Parse(((MainWindow)Application.Current.MainWindow).txtBombs.Text) + 2;

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
        // Tracks flags placed and stops user from using more than max
        // If all bombs are flagged play victory sound and display score
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
            else if (chosenCell.Flagged == false && chosenCell.CellValue == 9)
            {
                current_score = current_score + 1;
                chosenCell.Flagged = true;
                FlagCounter++;
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
