using System;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
            cmbRole.ItemsSource = new string[] { "Admin", "Manager", "User" };
            cmbRole.SelectedIndex = 2;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string role = cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : "User";

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool ok = Db.RegisterUser(username, password, fullName, role);
                if (ok)
                {
                    MessageBox.Show("Registered successfully. Please login now.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoginWindow login = new LoginWindow();
                    login.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

    }
}