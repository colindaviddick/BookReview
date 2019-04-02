using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace BookReview
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Content.ToString() == "Login")
            {
                //System.Windows.Forms.MessageBox.Show("Login");

                // new MainWindow().Show();
                #region Copied from old app

                SqlConnection sqlCon = new SqlConnection(@"Data source=COLIN\SQLMAIN; Initial Catalog=TestLoginCredentials; Integrated Security=True;");

                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    String query = "SELECT COUNT(1) FROM tblUser WHERE Username=@UserName AND Password=@Password";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                    int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    if (count == 1)
                    {
                        MainWindow dashboard = new MainWindow();
                        dashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Username or password was incorrect.");
                    }


                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();
                    // this.Close();
                }

            }


            #endregion

            else if (Login.Content.ToString() == "Register")
            {
                // System.Windows.Forms.MessageBox.Show("Register");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("I don't know what you've done.");
            

                //SqlConnection sqlCon = new SqlConnection(@"Data source=COLIN\SQLMAIN; Initial Catalog=TestLoginCredentials; Integrated Security=True;");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChkRegister_Unchecked(object sender, RoutedEventArgs e)
        {
            Login.Content = "Login";
        }

        private void ChkRegister_Checked(object sender, RoutedEventArgs e)
        {
            Login.Content = "Register";
        }

        private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (System.Windows.Forms.Control.IsKeyLocked(Keys.CapsLock))
            {
                CapsCheck.Visibility = Visibility.Visible;
            }
            else
            {
                // System.Windows.MessageBox.Show("The Caps Lock key is OFF.");
                CapsCheck.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (System.Windows.Forms.Control.IsKeyLocked(Keys.CapsLock))
            {
                CapsCheck.Visibility = Visibility.Visible;
            }
            else
            {
                // System.Windows.MessageBox.Show("The Caps Lock key is OFF.");
                CapsCheck.Visibility = Visibility.Collapsed;
            }
        }
    }
}

