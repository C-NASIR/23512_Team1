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
using System.Globalization;
using System.Drawing;
using System.IO;
using Brushes = System.Windows.Media.Brushes;

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
        public int win_score;

        public string maxScore
        {
            get { return txtBombs.Text; }
            set { txtBombs.Text = value; }
        }

        public int current_score;

        //max bombs set to 1/3 of board size
        public int max_bombs;

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        /// <summary>
        /// This Method Creates Dynamic Grid by using the number of rows and columns the user entered
        /// </summary>
        public Grid DynamicGridCreator(int numrows, int numColumns)
        {
            //initiating the new dynamic grid: giving some values and setting some properties
            Grid dynamicGrid = new Grid();
            dynamicGrid.Name = "gameBoard";
            dynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            dynamicGrid.VerticalAlignment = VerticalAlignment.Center;
            dynamicGrid.ShowGridLines = false;
            dynamicGrid.Background = System.Windows.Media.Brushes.LightGray;


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
                    btn.Background = System.Windows.Media.Brushes.LightGray;

                    //Creating the event signature
                    btn.Click += new RoutedEventHandler(btn_click);
                    btn.MouseRightButtonDown += new MouseButtonEventHandler(btn_rightClick);
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                    dynamicGrid.Children.Add(btn);
                }
            }


            //send dynamicGrid to the window to add to the window Grid
            return dynamicGrid;
        }

        //This is the click event of the dynamic event handler
        void btn_click(object sender, EventArgs e)
        {
            Button s = sender as Button;

            List<string> cells = game.ButtonLeftClicked(s.Name);

            if (game.GameEnd == true)
            {
                EndGame();
            }
            else
            {
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
                            b.Content = game.Game.Cells[cellLocationY, cellLocationX].CellDisplayValue;
                            //b.IsEnabled = false;
                            b.Click -= btn_click;
                            b.Click += btn_unclick;
                            b.Background = Brushes.Yellow;
                            break;
                        }
                    }
                }
            }
        }

        void btn_unclick(object sender, EventArgs e)
        {
            
        }

        //This is the click event of the dynamic event handler
        void btn_rightClick(object sender, EventArgs e)
        {
            Button s = sender as Button;
            bool isFlagged = game.ButtonRightClicked(s.Name);

            if (isFlagged == true)
            {
                s.Click -= btn_click;

                // changes background of the button to the flag image
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("../../Icons/flag.png", UriKind.Relative));
                s.Background = brush;
            }
            else
            {
                s.Click += btn_click;
                s.Background = System.Windows.Media.Brushes.LightGray;
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
                game = new GameLogic(int.Parse(txtWidth.Text), int.Parse(txtHeight.Text), int.Parse(txtBombs.Text));
            }
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
                        Window(x, y);
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
            windowGrid.ShowGridLines = false;
            windowGrid.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            windowGrid.Name = "windowGrid";

            //Define the number of columns in the window (1) at the width of the window
            ColumnDefinition column = new ColumnDefinition();
            windowGrid.ColumnDefinitions.Add(column);

            //Define the number of rows in the window (2) 
            //First row at height of the status bar (to be defined next)
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(0,GridUnitType.Auto);
            windowGrid.RowDefinitions.Add(row1);

            //Second row to fill the remainder of the window height
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(10, GridUnitType.Auto);
            windowGrid.RowDefinitions.Add(row2);

            //Third row for game over buttons
            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(100);
            windowGrid.RowDefinitions.Add(row3);

            //Row and buttons used for navigation bar
            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(0, GridUnitType.Auto);
            windowGrid.RowDefinitions.Add(row4);

            //screenshot button for navbar
            Button btnScreenshot = new Button();
            btnScreenshot.Click += screenCapture_Click;
            btnScreenshot.Height = 25;
            btnScreenshot.Width = 100;
            btnScreenshot.Padding = new Thickness(0, 0, 0, 0);
            btnScreenshot.Margin = new Thickness(50, 0, -50, 0);
            btnScreenshot.Content = "Screenshot";

            //rules button for nav bar
            Button btnRules = new Button();
            btnRules.Height = 25;
            btnRules.Width = 50;
            btnRules.Content = "Rules";
            btnRules.Click += rules_Click;

            //credit button for nav bar
            Button btnCredits = new Button();
            btnCredits.Height = 25;
            btnCredits.Width = 50;
            btnCredits.Content = "Credits";
            btnCredits.Click += credits_Click;

            //close game button for nav bar
            Button btnNavClose = new Button();
            btnNavClose.Click += closeGame_Click;
            btnNavClose.Height = 25;
            btnNavClose.Width = 50;
            btnNavClose.Content = "Close";

            //replay button for nav bar
            Button btnNavReplay = new Button();
            btnNavReplay.Click += newGame_Click;
            btnNavReplay.Height = 25;
            btnNavReplay.Width = 50;
            btnNavReplay.Content = "Replay";

            //stack panel to contain navigation elements
            StackPanel navStack = new StackPanel();
            navStack.Orientation = Orientation.Vertical;
            navStack.Width = this.Width;
            navStack.Height = 500;

            //wrap panel to contain first row of nav bar elements, rules and credits buttons
            WrapPanel navWrap = new WrapPanel();
            navWrap.HorizontalAlignment = HorizontalAlignment.Left;
            navWrap.Orientation = Orientation.Vertical;
            navWrap.Height = 31;
            navWrap.Width = 100;
            navWrap.Margin = new Thickness((this.Width / 2) - btnRules.Width, 0, 0, 0);

            //wrap panel for second row of nav bar elements, close and replay buttons
            WrapPanel navWrap2 = new WrapPanel();
            navWrap2.HorizontalAlignment = HorizontalAlignment.Left;
            navWrap2.Height = 31;
            navWrap2.Width = 100;
            navWrap2.Margin = new Thickness((this.Width / 2) - btnNavClose.Width, 0, 0, 0);

            //wrap panel for third row of nav bar elements, screenshot button
            WrapPanel navWrap3 = new WrapPanel();
            navWrap3.HorizontalAlignment = HorizontalAlignment.Left;
            navWrap3.Height = 31;
            navWrap3.Width = 100;
            navWrap3.Margin = new Thickness((this.Width / 2) - btnScreenshot.Width, 0, 0, 0);

            //add respective elements to each wrap panel
            navWrap.Children.Add(btnRules);
            navWrap.Children.Add(btnCredits);
            navWrap2.Children.Add(btnNavClose);
            navWrap2.Children.Add(btnNavReplay);
            navWrap3.Children.Add(btnScreenshot);

            //add wrap panels to the navStack stack panel
            navStack.Children.Add(navWrap);
            navStack.Children.Add(navWrap2);
            navStack.Children.Add(navWrap3);

            //column/row for navStack
            Grid.SetColumn(navStack, 0);
            Grid.SetRow(navStack, 2);
            windowGrid.Children.Add(navStack);
            //End of row and buttons used for navigation bar

            //Stackpanel for post game labels and buttons
            StackPanel endGameStack = new StackPanel();
            endGameStack.Visibility = Visibility.Collapsed;
            endGameStack.Width = this.Width;
            endGameStack.Height = 400;
            endGameStack.Tag = "check";

            //btn for replay in post game
            Button btnReplay = new Button();
            btnReplay.Click += newGame_Click;
            btnReplay.Height = 25;
            btnReplay.Width = 50;
            btnReplay.Content = "Replay";

            //btn for closing game in post game
            Button btnClose = new Button();
            btnClose.Click += closeGame_Click;
            btnClose.Height = 25;
            btnClose.Width = 50;
            btnClose.Content = "Close";

            //lbl for displaying final score in post game
            Label lblScore = new Label();
            lblScore.Name = "scoreLabel";
            lblScore.Content = current_score;
            lblScore.Height = 30;
            lblScore.Width = this.Width;
            lblScore.HorizontalContentAlignment = HorizontalAlignment.Center;

            //wrappanel for horizontaly aligning post game buttons
            WrapPanel endGameWrap = new WrapPanel();
            endGameWrap.HorizontalAlignment = HorizontalAlignment.Left;
            endGameWrap.Height = 31;
            endGameWrap.Width = 100;
            endGameWrap.Margin = new Thickness((this.Width / 2) - btnReplay.Width, 0, 0, 0);

            //add controls to stackpanel
            endGameStack.Children.Add(lblScore);
            endGameWrap.Children.Add(btnReplay);
            endGameWrap.Children.Add(btnClose);
            endGameStack.Children.Add(endGameWrap);

            //set column and row for endGameStack
            Grid.SetColumn(endGameStack, 0);
            Grid.SetRow(endGameStack, 3);
            windowGrid.Children.Add(endGameStack);

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
            statusGrid.ShowGridLines = false;
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

            //btn for displaying post game menu
            Button btnCheckScore = new Button();
            btnCheckScore.Height = 25;
            btnCheckScore.Width = 75;
            btnCheckScore.Content = "Smileface";
            btnCheckScore.Click += checkScore_Click;

            //insert the happiest of faces

            Grid.SetColumn(btnCheckScore, 1);
            Grid.SetRow(btnCheckScore, 0);
            statusGrid.Children.Add(btnCheckScore);

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

        /// <summary>
        /// Used as the timer for the player's score
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer(object sender, EventArgs e)
        {
            foreach (Label l in controls.OfType<Label>())
            {
                if (l.Name == "Timer")
                {
                    l.Content = getTime().ToString("HH:mm:ss");
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the elapsed.
        /// </summary>
        /// <returns></returns>
        public DateTime getTime()
        {
            TimeSpan elapsed = DateTime.Now - StartTime;
            return new DateTime().Add(elapsed);
        }

        /// <summary>
        /// Start application over via navigation menu
        /// </summary>
        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            //close grid and show start menu again, right now simply closes application and reopens
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Close the game via navigation menu
        /// </summary>
        private void closeGame_Click(object sender, RoutedEventArgs e)
        {
            //close application
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Capture screenshots via navigation menu
        /// Uses bitmap to take screenshot
        /// </summary>
        private void screenCapture_Click(object sender, RoutedEventArgs e)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    string screenPath = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + "\\images" + "\\" + filename;
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save(screenPath);
                    Opacity = 1;
                }
            }
        }
        /// <summary>
        /// Click event for credits, will display some information about the team members
        /// </summary>
        private void credits_Click(object sender, RoutedEventArgs e)
        {
            //Create the Window Base Grid
            Grid creditGrid = new Grid();
            creditGrid.HorizontalAlignment = HorizontalAlignment.Center;
            creditGrid.VerticalAlignment = VerticalAlignment.Center;
            creditGrid.ShowGridLines = false;
            creditGrid.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            creditGrid.Name = "windowGrid";

            //Define the number of columns in the window (1) at the width of the window
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(this.Width);
            creditGrid.ColumnDefinitions.Add(column);

            //Define the number of rows in the window (2) 
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(50);
            creditGrid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(this.Height - 150);
            creditGrid.RowDefinitions.Add(row2);

            //label for information on creators, will add more as bios are created
            Label lblCreator = new Label();
            lblCreator.Content = "Heres a little more about the creators of this game";

            //return to game board
            Button btnReturn = new Button();
            btnReturn.Content = "Return";
            btnReturn.Click += return_Click;
            btnReturn.Height = 31;
            btnReturn.Width = 50;

            //column/row for elements
            Grid.SetColumn(lblCreator, 0);
            Grid.SetRow(lblCreator, 0);
            Grid.SetColumn(btnReturn, 1);
            Grid.SetRow(btnReturn, 1);

            creditGrid.Children.Add(lblCreator);
            creditGrid.Children.Add(btnReturn);
            //adding the dynamic grid to the mainwindow
            Content = creditGrid;

            controls = FindVisualChildren<Control>(Application.Current.MainWindow);
        }

        /// <summary>
        /// Click event for displaying rules of minesweeper, possibly link to wikipedia rules article
        /// </summary>
        private void rules_Click(object sender, RoutedEventArgs e)
        {
            //Create the Window Base Grid
            Grid rulesGrid = new Grid();
            rulesGrid.HorizontalAlignment = HorizontalAlignment.Center;
            rulesGrid.VerticalAlignment = VerticalAlignment.Center;
            rulesGrid.ShowGridLines = false;
            rulesGrid.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            rulesGrid.Name = "windowGrid";

            //Define the number of columns in the window (1) at the width of the window
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(this.Width);
            rulesGrid.ColumnDefinitions.Add(column);

            //Define the number of rows in the window (2) 
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(50);
            rulesGrid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(this.Height - 150);
            rulesGrid.RowDefinitions.Add(row2);

            //label for information on game rules, will expand the information
            Label lblRules = new Label();
            lblRules.Content = "Rules of Minesweeper";

            //return to game board
            Button btnReturn = new Button();
            btnReturn.Content = "Return";
            btnReturn.Click += return_Click;
            btnReturn.Height = 31;
            btnReturn.Width = 50;

            //column/row for elements
            Grid.SetColumn(lblRules, 0);
            Grid.SetRow(lblRules, 0);
            Grid.SetColumn(btnReturn, 1);
            Grid.SetRow(btnReturn, 1);

            rulesGrid.Children.Add(lblRules);
            rulesGrid.Children.Add(btnReturn);

            //adding the dynamic grid to mainwindow
            Content = rulesGrid;

            controls = FindVisualChildren<Control>(Application.Current.MainWindow);
        }

        /// <summary>
        /// Click event for checking score and ending game
        /// </summary>
        private void checkScore_Click(object sender, RoutedEventArgs e)
        {
            EndGame();
            //calls to dynamically created grid elements not working
            //object check = (e.Source as Button).Tag;
            //endGameStack.Vistibility = Visibility.Visible;

        }


        /// <summary>
        /// Process the various elements when the game is over
        /// </summary>
        private void EndGame()
        {
            RevealBoard();
            int score = game.CalculateScore(getTime().ToString("HH:mm:ss"));

        }

        /// <summary>
        /// Reveals the board when the game is over
        /// </summary>
        private void RevealBoard()
        {
            foreach (Cell c in game.Game.Cells)
            {

                foreach (Button b in controls.OfType<Button>())
                {
                    if (b.Name == "btn_" + c.YLocation + "_" + c.XLocation)
                    {
                        b.Content = game.Game.Cells[c.YLocation, c.XLocation].CellDisplayValue;
                        b.IsEnabled = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Click event for returning to game board, will not keep progress towards game
        /// </summary>
        private void return_Click(object sender, RoutedEventArgs e)
        {
            if (Inputchecker(txtWidth.Text, txtHeight.Text, txtBombs.Text))
            {
                game = new GameLogic(int.Parse(txtWidth.Text), int.Parse(txtHeight.Text), int.Parse(txtBombs.Text));
            }
        }
    }
}
