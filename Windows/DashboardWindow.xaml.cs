using MFA.Classes;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows;

namespace MFA.Windows
{
    public partial class DashboardWindow : Window
    {
        private User currentUser;

        public DashboardWindow()
        {
            InitializeComponent();
            LoadStudents();
            LoadStudentsTodayChart();
            LoadStudentPieChart();
            SetupPermissions();
        }

        public DashboardWindow(User user) : this()
        {
            currentUser = user;
            lblWelcome.Content = $"Welcome {currentUser.FullName} ({currentUser.Role})";
            SetupPermissions();
        }
        private void LoadStudentsTodayChart()
        {
            var model = new OxyPlot.PlotModel { Title = "Students added today" };

            var lineSeries = new OxyPlot.Series.LineSeries
            {
                Title = "Logins",
                MarkerType = OxyPlot.MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyPlot.OxyColors.Lime
            };
            lineSeries.Points.Add(new OxyPlot.DataPoint(8, 5));   // 8 AM → 5 students
            lineSeries.Points.Add(new OxyPlot.DataPoint(10, 12)); // 10 AM → 12 students
            lineSeries.Points.Add(new OxyPlot.DataPoint(12, 20)); // 12 PM → 20 students
            lineSeries.Points.Add(new OxyPlot.DataPoint(14, 15)); // 2 PM → 15 students
            lineSeries.Points.Add(new OxyPlot.DataPoint(16, 18)); // 4 PM → 18 students
            lineSeries.Points.Add(new OxyPlot.DataPoint(18, 10)); // 6 PM → 10 students

            model.Series.Add(lineSeries);

            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Hour of Day",
                Minimum = 6,
                Maximum = 20,
                MajorStep = 2
            });

            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Students",
                Minimum = 0
            });

            StudentsTodayChart.Model = model;
        }

        private void LoadStudentPieChart()
        {
            var model = new OxyPlot.PlotModel { Title = "Registrations (Last 6 Months)" };

            var pie = new OxyPlot.Series.PieSeries
            {
                InsideLabelPosition = 0.6,
                StrokeThickness = 0.5
            };

            pie.Slices.Add(new OxyPlot.Series.PieSlice("March", 30) { Fill = OxyPlot.OxyColors.Lime });
            pie.Slices.Add(new OxyPlot.Series.PieSlice("April", 15) { Fill = OxyPlot.OxyColors.DodgerBlue });
            pie.Slices.Add(new OxyPlot.Series.PieSlice("May", 10) { Fill = OxyPlot.OxyColors.Orange });
            pie.Slices.Add(new OxyPlot.Series.PieSlice("June", 15) { Fill = OxyPlot.OxyColors.IndianRed });
            pie.Slices.Add(new OxyPlot.Series.PieSlice("July", 20) { Fill = OxyPlot.OxyColors.CadetBlue });
            pie.Slices.Add(new OxyPlot.Series.PieSlice("August", 10) { Fill = OxyPlot.OxyColors.LightGreen });
            
            model.Series.Add(pie);
            StudentPieChart.Model = model;
        }
        private void SetupPermissions()
        {
            if (currentUser == null) return;

            btnAddStudent.IsEnabled = false;
            btnUpdateStudent.IsEnabled = false;
            btnDeleteStudent.IsEnabled = false;

            if (currentUser.Role == "Admin")
            {
                btnAddStudent.IsEnabled = true;
                btnUpdateStudent.IsEnabled = true;
                btnDeleteStudent.IsEnabled = true;
            }
            else if (currentUser.Role == "Manager")
            {
                btnAddStudent.IsEnabled = true;
                btnUpdateStudent.IsEnabled = true;
                btnDeleteStudent.IsEnabled = false;
            }
        }

        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow win = new AddStudentWindow();
            if (win.ShowDialog() == true)
            {
                LoadStudents();
            }
        }

        private void btnUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            UpdateStudentWindow win = new UpdateStudentWindow();
            if (win.ShowDialog() == true)
            {
                LoadStudents();
            }
        }

        private void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            DeleteStudentWindow win = new DeleteStudentWindow();
            if (win.ShowDialog() == true)
            {
                LoadStudents();
            }
        }

        private void btnViewStudents_Click(object sender, RoutedEventArgs e)
        {
            ViewStudentsWindow win = new ViewStudentsWindow();
            win.Show();
        }

        private void LoadStudents()
        {
            List<Student> students = Db.GetStudentsPaged(1, 50);
            dgStudents.ItemsSource = students;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}