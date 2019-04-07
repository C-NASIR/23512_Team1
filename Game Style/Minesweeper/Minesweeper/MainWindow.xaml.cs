using System;
using System.Collections.Generic;
using System.Linq;
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
using Minesweeper.Pages;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Creating Login Page Object
        private LoginPage login;
        private GridPage grid;
        private NavPage Nav;
        public MainWindow()
        {
            InitializeComponent();
            login = new LoginPage();
            Content = login;
        }

       private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (login.submitted)
            {
                //changin the submit propery
                login.submitted = false;
                //initializing pages
                grid = new GridPage();
                Nav = new NavPage();
                grid.CreateTheGrid(login.columns,login.rows,login.mines);

                //Calling the Content Creator method
                Content = ContentCreator(Nav, grid);
            }
        }
        private Grid ContentCreator(NavPage Nav, GridPage theGrid)
        {
            Grid mGrid = new Grid();
            mGrid.Background = new SolidColorBrush(Colors.Aquamarine);
            mGrid.ShowGridLines = false;

            //Creating Rows
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(59);

            //Creating Rows
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(445);
            
            //adding the rows to the grid
            mGrid.RowDefinitions.Add(row1);
            mGrid.RowDefinitions.Add(row2);

            //creating the frames
            Frame navFrame = new Frame();
            navFrame.Name = "navFrame";
            navFrame.Content = Nav;
            Grid.SetRow(navFrame, 0);
            Frame gridFrame = new Frame();
            gridFrame.Name = "gridFrame";
            gridFrame.Content = theGrid;
            Grid.SetRow(gridFrame, 1);

            //Adding the frames to the grid
            mGrid.Children.Add(navFrame);
            mGrid.Children.Add(gridFrame);

            return mGrid;
        }
    }
}
