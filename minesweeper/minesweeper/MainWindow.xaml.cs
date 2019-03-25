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

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int BOXSIZE = 25;

        private GameLogic game;

        private IEnumerable<Control> controls;

        public MainWindow()
        {
            InitializeComponent();
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
        public  void DynamicGridCreator(int numrows, int numColumns)
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
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                    dynamicGrid.Children.Add(btn);
                }            
            }
            //adding the dynamic grid to the mainwindow
            Content = dynamicGrid;

            controls = FindVisualChildren<Control>(Application.Current.MainWindow);
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

                foreach (Button b in controls)
                {
                    if (b.Name == "btn_" + cellLocationY + "_" + cellLocationX)
                    {
                        b.Content = game.Game.Cells[cellLocationY, cellLocationX].CellDisplayValue;
                        AnimationBTN(b);
                        break;
                    }
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
                        DynamicGridCreator(x, y);
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
    }
}
