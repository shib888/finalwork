using final.ViewModel;
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

namespace final.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    

    public partial class Login : Page
    {
        private SharedViewModel _sharedViewModel;
        private UserViewModel _userViewModel;
        public Login(SharedViewModel _sharedViewModel)
        {
            InitializeComponent();
            this._sharedViewModel = _sharedViewModel;
            DataContext = _sharedViewModel;
        }



        private void Loginclick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the entered email and password
                string enteredEmail = UserName.Text;
                string enteredPassword = Passwordclass.Password;

                // Updated connection string with your database path
                string connectionString = @"Data Source=C:\Users\niv\source\repos\final\final\ViewModel\things.db; Version = 3; ";
                bool userFound = false; // Flag to indicate if user was found
                bool isAdmin = false; // Flag to indicate if user is an admin

                using (var connection = new System.Data.SQLite.SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to search for user
                    string sql = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password LIMIT 1";
                    using (var command = new System.Data.SQLite.SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", enteredEmail);
                        command.Parameters.AddWithValue("@Password", enteredPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                userFound = true; // Set the flag to true
                                                  // Check if the user is an admin
                                isAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                            }
                        }
                    }
                }

                // Check if a match was found and load the appropriate page
                if (userFound)
                {
                    if (isAdmin)
                    {
                        Admin newreg = new Admin(_sharedViewModel);
                        NavigationService.Navigate(newreg);
                      
                    }
                    else
                    {
                        // User is not an admin, load regular user page
                        MainWindow mainWithFrame = new MainWindow();
                        mainWithFrame.Show();


                    }
                }
                else
                {
                    // No match found, display an error message
                    MessageBox.Show("Invalid email or password. Please try again." + enteredEmail);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            Register newreg = new Register(_sharedViewModel);
            NavigationService.Navigate(newreg);
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
