using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();

            string cmndText = "SELECT employee_id, last_name, first_name, " +
                              "job_title, birth_date FROM employee";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    SqlDataReader reader = sqlCmnd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();

                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);

                        employeeList.Add(employee);
                    }
                }
            }
            catch
            {
                return employeeList = new List<Employee>();
            }
            return employeeList;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            List<Employee> searchedEmployees = new List<Employee>();

            string cmndText = "SELECT employee_id, first_name, last_name " +
                              "job_title, birth_date FROM employee";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    SqlDataReader reader = sqlCmnd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();

                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);

                        if (employee.LastName == lastname &&
                            employee.FirstName == firstname)
                        {
                            searchedEmployees.Add(employee);
                        }
                    }
                }
            }
            catch
            {
                searchedEmployees = new List<Employee>();
            }
            return searchedEmployees;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employeeListNoProjects = new List<Employee>();

            string cmndText = "SELECT employee_id, last_name, first_name, job_title, birth_date" +
                              " FROM employee WHERE employee_id NOT IN(SELECT employee_id" +
                              " FROM project_employee); ";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmd = new SqlCommand(cmndText, sqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader(); //reader contains the query results

                    while (reader.Read())
                    {
                        Employee employee = new Employee();

                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);

                        employeeListNoProjects.Add(employee);
                    }
                }
            }
            catch 
            {
                return employeeListNoProjects = new List<Employee>();
            }
            return employeeListNoProjects;
        }

    }
}
