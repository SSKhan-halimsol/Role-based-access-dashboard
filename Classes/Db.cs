using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MFA.Classes
{
    public static class Db
    {
        private static string connectionString = @"Server=192.168.1.188;Database=ShahbazApp;User Id=sa;Password=123456;";

        // Register user
        public static bool RegisterUser(string username, string password, string fullName, string role)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@RoleName", role);

                    SqlParameter returnParam = new SqlParameter();
                    returnParam.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnParam);

                    cmd.ExecuteNonQuery();
                    int result = (int)returnParam.Value;

                    return result == 1; // true if registration successful
                }
            }
        }

        // Login
        public static User SignIn(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LoginUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Username = username,
                            Role = reader["RoleName"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // Add student
        public static bool AddStudent(string rollNo, string name, string fatherName, DateTime enrollmentDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_AddStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RollNo", rollNo);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@FatherName", fatherName);
                    cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                    SqlParameter resultParam = new SqlParameter("@Result", System.Data.SqlDbType.Bit)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultParam);

                    cmd.ExecuteNonQuery();

                    return Convert.ToBoolean(resultParam.Value);
                }
            }
        }


        // Get students with pagination
        public static List<Student> GetStudentsPaged(int pageNumber, int pageSize, string searchTerm = "")
        {
            List<Student> students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudentsPaged", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            RollNo = reader["RollNo"].ToString(),
                            Name = reader["Name"].ToString(),
                            FatherName = reader["FatherName"].ToString(),
                            EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                        });
                    }
                }
            }

            return students;
        }

        // Update student
        public static bool UpdateStudent(string rollNo, string name, string fatherName, DateTime enrollmentDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", rollNo);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@FatherName", fatherName);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                SqlParameter returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParam.Value == 1;
            }
        }

        // Delete student
        public static bool DeleteStudent(string rollNo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", rollNo);

                SqlParameter returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParam.Value == 1;
            }
        }
    }
}