using System;
using System.Collections.Generic;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class UpdateStudentWindow : Window
    {
        private Dictionary<string, Student> studentsDict;

        public UpdateStudentWindow()
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
            List<Student> students = Db.GetStudentsPaged(1, 20);
            studentsDict = new Dictionary<string, Student>();
            cmbRollNo.Items.Clear();

            foreach (var s in students)
            {
                cmbRollNo.Items.Add(s.RollNo);
                studentsDict[s.RollNo] = s;
            }

            if (cmbRollNo.Items.Count > 0)
                cmbRollNo.SelectedIndex = 0;
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

        private void btnUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRollNo.SelectedItem == null)
            {
                MessageBox.Show("Select a student first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string rollNo = cmbRollNo.SelectedItem.ToString();
            string name = txtName.Text.Trim();
            string fatherName = txtFatherName.Text.Trim();
            DateTime? enrollmentDate = dpEnrollmentDate.SelectedDate;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(fatherName) || !enrollmentDate.HasValue)
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool result = Db.UpdateStudent(rollNo, name, fatherName, enrollmentDate.Value);
                if (result)
                {
                    MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}