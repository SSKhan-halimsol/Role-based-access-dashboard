using System;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class AddStudentWindow : Window
    {
        public AddStudentWindow()
        {
            InitializeComponent();
            dpEnrollmentDate.SelectedDate = DateTime.Now;
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dash = new DashboardWindow();
            dash.Show();
            this.Close();
        }
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            string rollNo = txtRollNo.Text.Trim();
            string name = txtName.Text.Trim();
            string fatherName = txtFatherName.Text.Trim();
            DateTime? enrollmentDate = dpEnrollmentDate.SelectedDate;

            if (string.IsNullOrEmpty(rollNo) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(fatherName) || !enrollmentDate.HasValue)
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool result = Db.AddStudent(rollNo, name, fatherName, enrollmentDate.Value);
                if (result)
                {
                    MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}