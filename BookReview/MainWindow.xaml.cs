#region Can forget about this section for now.....
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BookReview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size = 200;

        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = (@"Data Source=COLIN\SQLMAIN;Initial Catalog=TestLoginCredentials;Persist Security Info=True;User ID=sa;Password=Thr33four");
            sqlConnection = new SqlConnection(connectionString);

            ShowAuthors();
            ShowBooks();
        }

        #endregion

        #region        // These Methods Need work

        void AddBook(object sender, RoutedEventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }

            try
            {
                // If Author already exists, get ID, add AuthorID + BookID to BookAuthor table. Lol
                // Otherwise add new AuthorID & Book ID to table.
                string authorQuery = "insert into Author values (@AuthorName); insert into Book values (@Title, @Synopsis)";

                SqlCommand sqlCommand = new SqlCommand(authorQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AuthorName", newAuthor.Text);
                sqlCommand.Parameters.AddWithValue("@Title", newBookTitle.Text);
                sqlCommand.Parameters.AddWithValue("@Synopsis", newSynopsis.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong..." + ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowBooks();
                ShowAuthors();
            }
        }


        private void ListBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                string queryTitle = "select Title from Book select Synopsis from Book where Id = @BookID";
                //string queryAuthor = "select AuthorName from Author where Id = @AuthorID";

                SqlCommand sqlCommand = new SqlCommand(queryTitle, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);


                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@BookId", listBooks.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@Synopsis", listBooks.SelectedValue);

                    DataTable bookIDTable = new DataTable();
                    DataTable synopsisTable = new DataTable();

                    sqlDataAdapter.Fill(bookIDTable);
                    sqlDataAdapter.Fill(synopsisTable);

                    bookTitle.Text = bookIDTable.Rows[0]["Title"].ToString();
                    synopsis.Text = synopsisTable.Rows[0]["Synopsis"].ToString();


                    // sqlCommand.Parameters.AddWithValue("@AuthorId", listBooks.SelectedValue);
                    // DataTable authorIDTable = new DataTable();
                    // sqlDataAdapter.Fill(authorIDTable);
                    // bookTitle.Text = bookIDTable.Rows[0]["AuthorName"].ToString();
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show("ShowSelectedAnimalInTextBox" + e2.ToString());
            }
        }


        #endregion
        #region       // Not working at all
        private void Delete_Book(object sender, RoutedEventArgs e)
        {
            // Causing problems with Foreign Keys... Need to update BookAuthor table too??

            try
            {
                string query = "delete from Book where Id = @Title";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Title", listBooks.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Book" + ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowBooks();
            }
        }

        private void Delete_Author(object sender, RoutedEventArgs e)
        {

        }


        #endregion

        #region Can forget about these
        // Perfect
        void ShowAuthors()
        {
            try
            {
                string query = "SELECT * FROM Author ORDER BY AuthorName";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    var authorDataTable = new DataTable();
                    sqlDataAdapter.Fill(authorDataTable);

                    listAuthors.DisplayMemberPath = "AuthorName";
                    listAuthors.SelectedValuePath = "Id";
                    listAuthors.ItemsSource = authorDataTable.DefaultView;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
            }
        }

        void ShowBooks()
        {
            try
            {
                string query = "SELECT * FROM Book ORDER BY Title";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    var bookDataTable = new DataTable();
                    sqlDataAdapter.Fill(bookDataTable);

                    listBooks.DisplayMemberPath = "Title";
                    listBooks.SelectedValuePath = "Id";
                    listBooks.ItemsSource = bookDataTable.DefaultView;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
            }
        }

        private void ListAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string query = "select * from Book b inner join BookAuthor ba on b.Id = ba.BookId where ba.AuthorId = @AuthorId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@AuthorId", listAuthors.SelectedValue);

                    DataTable bookDataTable = new DataTable();

                    sqlDataAdapter.Fill(bookDataTable);

                    //Which Information of the Table in DataTable should be shown in our ListBox?
                    listBooks.DisplayMemberPath = "Title";
                    //Which Value should be delivered, when an Item from our ListBox is selected?
                    listBooks.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listBooks.ItemsSource = bookDataTable.DefaultView;
                }
            }
            catch (Exception e2)
            {
                // Causes some issues.

                // MessageBox.Show("ShowAssociatedAnimals" + e2.ToString());
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Am I bothering you? Do you want me to leave?",
                "The Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            //
            // Test the results of the previous three dialogs.
            //
            if (result.ToString() == "Yes")
            {
                MessageBox.Show("You answered yes, so now I must go. Goodbye.");
                this.Close();
            }
        }
    }
}
#endregion
