using System;
using System.Collections.Generic;
using System.Linq;
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
        public void DynamicGridCreator(int numrows, int numColumns)
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
                    btn.Name = "btn" + x + y;
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                    dynamicGrid.Children.Add(btn);
                }            
            }

            //adding the dynamic grid to the mainwindow
            this.Content = dynamicGrid;
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
            if (txtHeight.Text == "" || txtWidth.Text == "" || txtHeight.Text.Contains("H") == true || txtWidth.Text.Contains("W") == true)
            {
                MessageBox.Show("Please enter a grid height and width.");

            }
            else
            {
                //If input is correct users will be allowed to enter # of bombs
                game_height = int.Parse(txtHeight.Text);
                game_width = int.Parse(txtWidth.Text);
                max_bombs = (game_height * game_width) / 3;
                MessageBox.Show("You selected a " + game_height + " by " + game_width + " board.");
                GridSize.IsEnabled = false;
                grpBombs.IsEnabled = true;
                grpBombs.Visibility = Visibility.Visible;
            }
        }

        /*Click event for bomb selection. Will check for proper input before allowing
        * the user to create grid.*/
        private void btnBombs_Click(object sender, RoutedEventArgs e)
        {
            if (txtBombs.Text == "" || txtBombs.Text.Contains("B") == true)
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
                MessageBox.Show("You selected " + game_bombs + " bombs on a " + game_height + " by " + game_width + " board.");
                BombChoice.IsEnabled = false;
                SubmitBtn.Visibility = Visibility.Visible;
            }
        }

        private void SubmitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: instantiate GameLogic class and move code below to game logic

            //Creating variables to hold the numbers
            int numRow, numCol;


            //checking if the user entered legit number of columns
            if (int.TryParse(txtWidth.Text, out numCol))
            {
                //Assigning the number of columns
                numCol = int.Parse(txtWidth.Text);

                //checking if the user entered legit number of rows
                if (int.TryParse(txtHeight.Text, out numRow))
                {
                    //assigning the number of rows
                    numRow = int.Parse(txtHeight.Text);

                    //Calling the dynamic grid creator method
                    DynamicGridCreator(numRow, numCol);
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
    }
}
