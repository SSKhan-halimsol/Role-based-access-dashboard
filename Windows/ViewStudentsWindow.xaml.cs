using System.Collections.Generic;
using System.Windows;
using MFA.Classes;

namespace MFA.Windows
{
    public partial class ViewStudentsWindow : Window
    {
        public ViewStudentsWindow()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            List<Student> students = Db.GetStudentsPaged(1, 20);
            dgStudents.ItemsSource = students;
        }
        private void return_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dash = new DashboardWindow();
            dash.Show();
            this.Close();
        }
    }
}