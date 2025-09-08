using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class StudentListWindow : Window
    {
        private List<Student> allStudents;
        private List<Student> filteredStudents;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;

        public StudentListWindow()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            allStudents = Db.GetStudentsPaged(1, 20);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            filteredStudents = allStudents;

            if (!string.IsNullOrEmpty(txtFilterRollNo.Text))
                filteredStudents = filteredStudents.Where(s => s.RollNo.Contains(txtFilterRollNo.Text.Trim())).ToList();

            if (!string.IsNullOrEmpty(txtFilterName.Text))
                filteredStudents = filteredStudents.Where(s => s.Name.IndexOf(txtFilterName.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (!string.IsNullOrEmpty(txtFilterFatherName.Text))
                filteredStudents = filteredStudents.Where(s => s.FatherName.IndexOf(txtFilterFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (dpFromDate.SelectedDate.HasValue)
                filteredStudents = filteredStudents.Where(s => s.EnrollmentDate >= dpFromDate.SelectedDate.Value).ToList();

            if (dpToDate.SelectedDate.HasValue)
                filteredStudents = filteredStudents.Where(s => s.EnrollmentDate <= dpToDate.SelectedDate.Value).ToList();

            totalPages = (int)Math.Ceiling(filteredStudents.Count / (double)pageSize);
            currentPage = 1;
            DisplayPage();
        }

        private void DisplayPage()
        {
            var pageData = filteredStudents.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            dgStudents.ItemsSource = pageData;
            txtPageInfo.Text = $"Page {currentPage} of {totalPages}";
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}