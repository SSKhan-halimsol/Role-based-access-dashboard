using System;
using System.Collections.Generic;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class DeleteStudentWindow : Window
    {
        private Dictionary<string, Student> studentsDict;
        private int currentPage = 1;
        private int pageSize = 10; // students per page
        private string searchTerm = ""; // optional for filtering

        public DeleteStudentWindow()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dash = new DashboardWindow();
            dash.Show();
            this.Close();
        }
        private void LoadStudents()
        {
            List<Student> students = Db.GetStudentsPaged(currentPage, pageSize, searchTerm);
            studentsDict = new Dictionary<string, Student>();
            cmbRollNo.Items.Clear();

            foreach (var s in students)
            {
                cmbRollNo.Items.Add(s.RollNo);
                studentsDict[s.RollNo] = s;
            }

            if (cmbRollNo.Items.Count > 0)
                cmbRollNo.SelectedIndex = 0;

            txtPageInfo.Text = $"Page {currentPage}";
        }

        private void cmbRollNo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbRollNo.SelectedItem == null) return;

            string rollNo = cmbRollNo.SelectedItem.ToString();
            if (studentsDict.ContainsKey(rollNo))
            {
                var student = studentsDict[rollNo];
                txtName.Text = student.Name;
                txtFatherName.Text = student.FatherName;
                dpEnrollmentDate.SelectedDate = student.EnrollmentDate;
            }
        }

        private void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRollNo.SelectedItem == null)
            {
                MessageBox.Show("Select a student first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string rollNo = cmbRollNo.SelectedItem.ToString();

            var confirm = MessageBox.Show($"Are you sure you want to delete student with Roll No {rollNo}?",
                                          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    bool result = Db.DeleteStudent(rollNo);
                    if (result)
                    {
                        MessageBox.Show("Student deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStudents();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            LoadStudents();
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadStudents();
            }
        }
    }
}