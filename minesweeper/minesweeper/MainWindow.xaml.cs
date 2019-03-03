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

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

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
                column.Width = new GridLength(25);
                dynamicGrid.ColumnDefinitions.Add(column);
            }

            /*This for loop is getting the user from the number of rows
             and making that number of rows for the grid and adding it*/
            for (int i = 0; i < numrows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(25);
                dynamicGrid.RowDefinitions.Add(row);
            }

            //adding the dynamic grid to the mainwindow
            this.Content = dynamicGrid;
        }

        private void SubmitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //Creating variables to hold the numbers
            int numrows, numcolumns;


            //checking if the user entered legit number of columns
            if (int.TryParse(ColTbx.Text, out numcolumns))
            {
                //Assigning the number of columns
                numcolumns = int.Parse(ColTbx.Text);

                //checking if the user entered legit number of rows
                if (int.TryParse(RowTbx.Text, out numrows))
                {
                    //assigning the number of rows
                    numrows = int.Parse(RowTbx.Text);

                    //Calling the dynamic grid creator method
                    DynamicGridCreator(numrows, numcolumns);
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

        private void RowTbx_GotFocus(object sender, RoutedEventArgs e)
        {
            //clearing the rows textbox placeholder after it gets focus
            RowTbx.Text = "";
        }

        private void ColTbx_GotFocus(object sender, RoutedEventArgs e)
        {
            //clearing the columns textbox placeholder after it gets focus
            ColTbx.Text = "";
        }
    }
}
