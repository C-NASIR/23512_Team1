using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Threading;

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Grid gameGrid = new Grid();

        public const int BOXSIZE = 40;

        private GameLogic game;

        private IEnumerable<Control> controls;

        public DispatcherTimer clock;

        public DateTime StartTime;

        public MainWindow()
        {
            InitializeComponent();

            // Setup timer (clock)
            clock = new DispatcherTimer();
            clock.Tick += timer;
            clock.Interval = new TimeSpan(0, 0, 0, 0, 1);
            StartTime = DateTime.Now;
        }

        //pattern for numbers only
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text

        //variables to store user input
        public int game_height;
        public int game_width;
        public int game_bombs;

        //max bombs set to 1/3 of board size
        public int max_bombs;

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        /// <summary>
        /// This Method Creates Dynamic Grid by using the number of rows and columns the user entered
        /// </summary>
        public  Grid DynamicGridCreator(int numrows, int numColumns)
        {
            //initiating the new dynamic grid: giving some values and setting some properties
            Grid dynamicGrid = new Grid();
            dynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            dynamicGrid.VerticalAlignment = VerticalAlignment.Center;
            dynamicGrid.ShowGridLines = true;
            dynamicGrid.Background = new SolidColorBrush(Colors.AntiqueWhite);


            /*This loop is getting the user from the number of columns and makes that number 
            of columns for the grid and its adding it*/
            for (int i = 0; i < numColumns; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(BOXSIZE);
                dynamicGrid.ColumnDefinitions.Add(column);
            }

            /*This for loop is getting the user from the number of rows
             and making that number of rows for the grid and adding it*/
            for (int i = 0; i < numrows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(BOXSIZE);
                dynamicGrid.RowDefinitions.Add(row);
            }

            /*This loop populates the grid with buttons in the appropriate
             * location along with the appropriate name in relation to its
             * coordinates.*/
            for (int y = 0; y < numrows; y++)
            {
                for (int x = 0; x < numColumns; x++)
                {
                    Button btn = new Button();
                    btn.Name = "btn_" + y + "_" + x;

                    //Creating the event signature
                    btn.Click += new RoutedEventHandler(btn_click);
                    btn.MouseRightButtonDown += new MouseButtonEventHandler(btn_rightClick);   
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                    dynamicGrid.Children.Add(btn);
                }            
            }
            // add flag label to grid
            Label flagLabel = new Label();
            flagLabel.Content = "Flags: ";
            flagLabel.Width = 100;
            flagLabel.Height = 50;
            flagLabel.Margin = new Thickness(10);
            flagLabel.Foreground = new SolidColorBrush(Colors.White);
            flagLabel.Background = new SolidColorBrush(Colors.Black);
            
            //send dynamicGrid to the window to add to the window Grid
            return dynamicGrid;

            //controls = FindVisualChildren<Control>(Application.Current.MainWindow);
        }

        //This is the click event of the dynamic event handler
        void btn_click(object sender, EventArgs e)
        {
            Button s = sender as Button;

          
            List<string> cells = game.ButtonLeftClicked(s.Name);

            foreach (string n in cells)
            {

                int cellLocationX;
                int cellLocationY;
                string[] btnName = n.Split('_');
                int.TryParse(btnName[0], out cellLocationY);
                int.TryParse(btnName[1], out cellLocationX);
                
                foreach (Button b in controls.OfType<Button>())
                {
                    if (b.Name == "btn_" + cellLocationY + "_" + cellLocationX)
                    {
                        //b.Click -= btn_click;
                        //b.MouseRightButtonDown -= btn_rightClick;
                        b.Content = game.Game.Cells[cellLocationY, cellLocationX].CellDisplayValue;
                        AnimationBTN(b);
                        b.IsEnabled = false;
                        break;
                    }
                }
            }
        }

        //This is the click event of the dynamic event handler
        void btn_rightClick(object sender, EventArgs e)
        {
            Button s = sender as Button;
            s.Content = game.ButtonRightClicked(s.Name);
            if ((string)s.Content == "F")
            {
                s.Click -= btn_click;
            }
            else
            {
                s.Click += btn_click;
            }
            foreach (Label l in controls.OfType<Label>())
            {
                if (l.Name == "FlagCounter")
                {
                    if (game.FlagCounter == 0)
                    {
                        l.Content = "   Flags: ";
                    }
                    else
                    {
                        l.Content = "   Flags: " + game.FlagCounter;
                    }

                    break;
                }
            }
        }

        //Find all controls in the window
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        //Prevents improper input
        private void txtHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        //Prevents improper input
        private void txtWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        //Prevents improper input
        private void txtBombs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        //Button shows user txt input fields
        private void btnSetup_Click(object sender, RoutedEventArgs e)
        {
            btnSetup.Visibility = Visibility.Collapsed;
            grpGridSize.Visibility = Visibility.Visible;
            bkgGameOptions.Visibility = Visibility.Visible;
        }

        /*Click event for grid size selection. Will check for proper input before allowing
        * the user to select # of bombs.*/
        private void btnGridSize_Click(object sender, RoutedEventArgs e)
        {
            if (txtHeight.Text == "" || txtWidth.Text == "" || txtHeight.Text.Contains("H") || txtWidth.Text.Contains("W"))
            {
                MessageBox.Show("Please enter a grid height and width.");
            }
            else
            {
                //If input is correct users will be allowed to enter # of bombs
                game_height = int.Parse(txtHeight.Text);
                game_width = int.Parse(txtWidth.Text);
                max_bombs = (game_height * game_width) / 3;
                GridSize.IsEnabled = false;
                grpBombs.IsEnabled = true;
                grpBombs.Visibility = Visibility.Visible;
            }
        }

        /*Click event for bomb selection. Will check for proper input before allowing
        * the user to create grid.*/
        private void btnBombs_Click(object sender, RoutedEventArgs e)
        {
            if (txtBombs.Text == "" || txtBombs.Text.Contains("B"))
            {
                MessageBox.Show("Please enter a number of bombs.");
            }
            else if (int.Parse(txtBombs.Text) > max_bombs)
            {
                MessageBox.Show("This board only allows " + max_bombs + " bombs.");
            }
            else
            {
                //if input is correct users will be able to create grid.
                game_bombs = int.Parse(txtBombs.Text);
                BombChoice.IsEnabled = false;
                SubmitBtn.Visibility = Visibility.Visible;
            }
        }

        private void SubmitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //call the input checker method
            if (Inputchecker(txtWidth.Text, txtHeight.Text, txtBombs.Text))
            {
                game = new GameLogic(int.Parse(txtWidth.Text),int.Parse(txtHeight.Text),int.Parse(txtBombs.Text));
            }
        }

        //Button Animation method
        private void AnimationBTN(Button b)
        {
            b.Background = Brushes.Yellow;
        }

        //Removes label text from txtBox as user selects it
        private void txtHeight_GotFocus(object sender, RoutedEventArgs e)
        {
            txtHeight.Text = "";
        }

        //Removes label text from txtBox as user selects it
        private void txtWidth_GotFocus(object sender, RoutedEventArgs e)
        {
            txtWidth.Text = "";
        }

        //Removes label text from txtBox as user selects it
        private void txtBombs_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBombs.Text = "";
        }
        //Check the User input 
        public bool Inputchecker(string xNumber, string yNumber, string numMines)
        {
            bool checker = false;
            int x, y, numMine;
            //checking if the user entered legit number of columns
            if (int.TryParse(xNumber, out x))
            {
                //checking if the user entered legit number of rows
                if (int.TryParse(yNumber, out y))
                {
                    if (int.TryParse(numMines, out numMine))
                    {
                        //Calling the dynamic grid creator method
                        //DynamicGridCreator(x, y);
                        Window(x,y);
                        checker = true;
                    }
                    else
                    {
                        MessageBox.Show("Please Enter a Valid number of rows");
                    }
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

        public void Window(int x, int y)
        {
            //Create the Window Base Grid
            Grid windowGrid = new Grid();
            windowGrid.HorizontalAlignment = HorizontalAlignment.Center;
            windowGrid.VerticalAlignment = VerticalAlignment.Center;
            windowGrid.ShowGridLines = true;
            windowGrid.Background = new SolidColorBrush(Colors.DarkOliveGreen);

            //Define the number of columns in the window (1) at the width of the window
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(this.Width);
            windowGrid.ColumnDefinitions.Add(column);

            //Define the number of rows in the window (2) 
            //First row at height of the status bar (to be defined next)
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(50);
            windowGrid.RowDefinitions.Add(row1);

            //Second row to fill the remainder of the window height
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(this.Height - 50);
            windowGrid.RowDefinitions.Add(row2);

            //Create the  statusStrip grid
            Grid statusStrip = StatusBar();

            //Create the gameboard grid
            Grid gameBoard = DynamicGridCreator(x, y);

            //Define the window grid content column and row content
            Grid.SetColumn(statusStrip, 0);
            Grid.SetRow(statusStrip, 0);
            Grid.SetColumn(gameBoard, 0);
            Grid.SetRow(gameBoard, 1);

            //Add the statusBar
            windowGrid.Children.Add(statusStrip);

            //Add the gameboard
            windowGrid.Children.Add(gameBoard);

            //adding the dynamic grid to the mainwindow
            Content = windowGrid;

            controls = FindVisualChildren<Control>(Application.Current.MainWindow);

            // Game StartTime
            StartTime = DateTime.Now;
            clock.Start();
        }

        public Grid StatusBar()
        {
            //Create the status strip grid
            Grid statusGrid = new Grid();
            statusGrid.HorizontalAlignment = HorizontalAlignment.Center;
            statusGrid.VerticalAlignment = VerticalAlignment.Center;
            statusGrid.ShowGridLines = true;
            statusGrid.Background = new VisualBrush();

            //Define the number of columns in the window (3)
            //First column = 2/5ths of window
            ColumnDefinition status1 = new ColumnDefinition();
            status1.Width = new GridLength((this.Width / 5) * 2);
            statusGrid.ColumnDefinitions.Add(status1);

            //Second column = 1/5th of window
            ColumnDefinition status2 = new ColumnDefinition();
            status2.Width = new GridLength(this.Width / 5);
            statusGrid.ColumnDefinitions.Add(status2);

            //Third column = 2/5ths of window
            ColumnDefinition status3 = new ColumnDefinition();
            status3.Width = new GridLength((this.Width / 5) * 2);
            statusGrid.ColumnDefinitions.Add(status3);

            //Define the row (1) at size 50
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(50);
            statusGrid.RowDefinitions.Add(row1);

            // Create Flag Counter Label
            Label flagLabel = new Label();
            flagLabel.Name = "FlagCounter";
            flagLabel.Content = "   Flags: ";
            flagLabel.Width = 100;
            flagLabel.Height = 50;
            flagLabel.FontSize = 16;
            flagLabel.VerticalContentAlignment = VerticalAlignment.Bottom;
            flagLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
            flagLabel.Margin = new Thickness(10, 0, 10, 0);
            flagLabel.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
            flagLabel.Background = new SolidColorBrush(Colors.White);
            flagLabel.BorderBrush = new SolidColorBrush(Colors.Black);


            //Add Flag Counter Label to statusGrid
            Grid.SetColumn(flagLabel, 0);
            Grid.SetRow(flagLabel, 0);
            statusGrid.Children.Add(flagLabel);

            // Create label to display timer
            Label timerLabel = new Label();
            timerLabel.Name = "Timer";
            timerLabel.Content = "00:00:00";
            timerLabel.Width = 100;
            timerLabel.Height = 50;
            timerLabel.FontSize = 16;
            timerLabel.VerticalContentAlignment = VerticalAlignment.Bottom;
            timerLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            timerLabel.Margin = new Thickness(10, 0, 10, 0);
            timerLabel.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
            timerLabel.Background = new SolidColorBrush(Colors.White);
            timerLabel.BorderBrush = new SolidColorBrush(Colors.Black);

            //Add timer label to statusGrid
            Grid.SetColumn(timerLabel, 2);
            Grid.SetRow(timerLabel, 0);
            statusGrid.Children.Add(timerLabel);

            //Return Grid
            return statusGrid;
        }

        private void timer(object sender, EventArgs e)
        {
            foreach (Label l in controls.OfType<Label>())
            {
                if (l.Name == "Timer")
                {
                    TimeSpan elapsed = DateTime.Now - StartTime;
                    DateTime x = new DateTime().Add(elapsed);
                    l.Content = x.ToString("HH:mm:ss");
                    break;
                }
            }
        }
    }
}
