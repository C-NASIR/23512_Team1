using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Minesweeper.Classes;

namespace Minesweeper
{
    class GridGenerator
    {

        private int numRows;
        private int numColumns;
        private const double BOXZISE = 30;
        private GameLogic Game;
        private List<Button> BtnList = new List<Button>();
        private IEnumerable<Control> controls;
        public  GridGenerator(int numColumns, int numRows, int numMine)
        {
            //Filing the properties
            this.numColumns = numColumns;
            this.numRows = numRows;
            Game = new GameLogic(numColumns,numRows,numMine);
        }

        /// <summary>
        /// This Method Creates Dynamic Grid by using the number of rows and columns the user entered
        /// </summary>
        public Grid DynamicGridCreator()
        {
            int numrows = this.numRows;
            int numColumns = this.numColumns;
            //initiating the new dynamic grid: giving some values and setting some properties
            Grid dynamicGrid = new Grid();
            dynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            dynamicGrid.VerticalAlignment = VerticalAlignment.Center;
            dynamicGrid.ShowGridLines = false;
            dynamicGrid.Background = new SolidColorBrush(Colors.AntiqueWhite);


            /*This loop is getting the user from the number of columns and makes that number 
            of columns for the grid and its adding it*/
            for (int i = 0; i < numColumns; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(BOXZISE);
                dynamicGrid.ColumnDefinitions.Add(column);
            }

            /*This for loop is getting the user from the number of rows
             and making that number of rows for the grid and adding it*/
            for (int i = 0; i < numrows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(BOXZISE);
                dynamicGrid.RowDefinitions.Add(row);
            }

            /*This loop populates the grid with buttons in the appropriate
             * location along with the appropriate name in relation to its
             * coordinates.*/
            for (int y = 0; y < numrows; y++)
            {
                for (int x = 0; x < numColumns; x++)
                {
                    dynamicGrid.Children.Add(CreateButton(x,y));
                }
            }


            //send dynamicGrid to the window to add to the window Grid
            return dynamicGrid;
        }


        private Button CreateButton(int x, int y)
        {
            Button btn = new Button();
            btn.Name = "btn_" + y + "_" + x;

            //Creating the event signature
             btn.Click +=new RoutedEventHandler(button_MouseLeftButtonUp);
             btn.MouseRightButtonUp += new MouseButtonEventHandler(button_MouseRightButtonUp);
            Grid.SetColumn(btn, x);
            Grid.SetRow(btn, y);
            
            //add teh button to the button list
            BtnList.Add(btn);
            return btn;
        }

        /// <summary>
        /// This Event Happenes when the user clicks the Left Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Button s = sender as Button;

            List<string> cells = Game.ButtonLeftClicked(s.Name);

            foreach (string n in cells)
            {

                int cellLocationX;
                int cellLocationY;
                string[] btnName = n.Split('_');
                int.TryParse(btnName[0], out cellLocationY);
                int.TryParse(btnName[1], out cellLocationX);

                foreach (Button b in BtnList.OfType<Button>())
                {
                    if (b.Name == "btn_" + cellLocationY + "_" + cellLocationX)
                    {
                        //b.Click -= btn_click;
                        //b.MouseRightButtonDown -= btn_rightClick;
                        b.Content = Game.Game.Cells[cellLocationY, cellLocationX].CellDisplayValue;
                        //AnimationBTN(b);
                        b.IsEnabled = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// This Event happens when the user clicks the Right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button s = sender as Button;
            s.Content = Game.ButtonRightClicked(s.Name);

            if ((string)s.Content == "F")
            {
                s.Click -= button_MouseLeftButtonUp;
            }
            else
            {
                s.Click += button_MouseLeftButtonUp;
            }
            foreach (Label l in BtnList.OfType<Label>())
            {
                if (l.Name == "FlagCounter")
                {
                    if (Game.FlagCounter == 0)
                    {
                        l.Content = "   Flags: ";
                    }
                    else
                    {
                        l.Content = "   Flags: " + Game.FlagCounter;
                    }

                    break;
                }
            }
        }
    }
}
