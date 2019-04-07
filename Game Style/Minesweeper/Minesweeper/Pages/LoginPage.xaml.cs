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
using Minesweeper;

namespace Minesweeper.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public bool submitted, focused_Clumn, focused_Row, focused_Mine;
        public int columns, rows,mines;
        public event EventHandler UserSubmitted;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void txtBoxClumns_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!focused_Clumn)
            {
                txtBoxClumns.Text = "";
                focused_Clumn = true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            submitted = true;

        }

        private void txtBoxClumns_LostFocus(object sender, RoutedEventArgs e)
        {
            //Calls the validate method
            string message;
            if (!validate(txtBoxClumns.Text, out message))
                tbkColumnError.Text = message;
            else
            {
                tbkColumnError.Text = "";
                columns = int.Parse(txtBoxClumns.Text);
            }

            //enabling the submit button
            if (columns > 0 && rows > 0 && mines > 0)
                btnSubmit.IsEnabled = true;
        }

        private void txtBoxRows_LostFocus(object sender, RoutedEventArgs e)
        {
            //Calls the validate method
            string message;
            if (!validate(txtBoxRows.Text, out message))
                tbkRowError.Text = message;
            else
            {
                tbkRowError.Text = "";
                rows = int.Parse(txtBoxClumns.Text);
            }

            //enabling the submit button
            if (columns > 0 && rows > 0 && mines > 0)
                btnSubmit.IsEnabled = true;
        }

        private void txtBoxRows_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!focused_Row)
            {
                txtBoxRows.Text = "";
                focused_Row = true;
            }
        }

        private void txtBoxMines_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!focused_Mine)
            {
                txtBoxMines.Text = "";
                focused_Mine = true;
            }
        }

        private void txtBoxMines_LostFocus(object sender, RoutedEventArgs e)
        {
            //Calls the validate method
            string message;
            if (!validate(txtBoxMines.Text, out message))
                tbkMineError.Text = message;
            else
            {
                if (!mineExtraValidate())
                {
                    tbkMineError.Text = "";
                    mines = int.Parse(txtBoxMines.Text);
                }
            }

            //enabling the submit button
            if (columns > 0 && rows > 0 && mines > 0)
                btnSubmit.IsEnabled = true;
        }


        //This method validates the input
        private bool validate(string value, out string message)
        {
            bool flag = false;
            int number;
            message = "";
            if (int.TryParse(value, out number))
            {
                if (2 < number && number < 25)
                    flag = true;
                else
                    message = "Please Enter a Number between 2 and 25";
            }
            else
            {
                message = "Please Enter an Integer Number";
            }

            return flag;
        }

        //This method does the extra validation for the mine
        private bool mineExtraValidate()
        {
            bool flag = false;
            int col, ro;
            string message;
            if (int.TryParse(txtBoxClumns.Text, out col) && int.TryParse(txtBoxRows.Text, out ro))
            {
                if ((int.Parse(txtBoxClumns.Text) + int.Parse(txtBoxRows.Text)) * 1 / 3 < int.Parse(txtBoxMines.Text))
                {
                    message = "Mines must be less than 1/3 of the cells";
                    tbkMineError.Text = message;
                    flag = true;
                }
            }
            else
            {
                message = "Mines must be less than 1/3 of the cells";
                tbkMineError.Text = message;
                flag = true;
            }

            return flag;
        }
    }
}
