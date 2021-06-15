using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            IList<Department> departmentList = new List<Department>();

            string cmndText = "SELECT department_id, name FROM department";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    SqlDataReader reader = sqlCmnd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department department = new Department();

                        department.Id = Convert.ToInt32(reader["department_id"]);
                        department.Name = Convert.ToString(reader["name"]);

                        departmentList.Add(department);
                    }
                }
            }
            catch
            {
                return departmentList = new List<Department>(); ;
            }
            return departmentList;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            int newId = 0;

            string cmndText = "INSERT INTO department (name) VALUES (@name)";
            string cmndText2 = "SELECT department_id FROM department WHERE " +
                               "name = @name";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    sqlCmnd.Parameters.AddWithValue("@name", newDepartment.Name);
                    int rowsAffected = sqlCmnd.ExecuteNonQuery();

                    SqlCommand sqlCmnd2 = new SqlCommand(cmndText2, sqlConn);
                    sqlCmnd2.Parameters.AddWithValue("@name", newDepartment.Name);
                    SqlDataReader reader = sqlCmnd2.ExecuteReader();

                    if (reader.Read())
                    {
                        newId = Convert.ToInt32(reader["department_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return newId;
            }
            return newId;
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool result = false;

            string cmndText = "UPDATE department SET name = @name WHERE" +
                              " department_id = @department_id";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    sqlCmnd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    sqlCmnd.Parameters.AddWithValue("@department_id", updatedDepartment.Id);
                    int rowsAffected = sqlCmnd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                return result;
            }
            return result;
        }
    }
}
