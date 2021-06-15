using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private readonly string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            List<Project> projectList = new List<Project>();

            string cmndTxt = "SELECT project_id, name, from_date, to_date FROM project";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndTxt, sqlConn);
                    SqlDataReader reader = sqlCmnd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project project = new Project();

                        project.ProjectId = Convert.ToInt32(reader["project_id"]);
                        project.Name = Convert.ToString(reader["name"]);
                        project.StartDate = Convert.ToDateTime(reader["from_date"]);
                        project.EndDate = Convert.ToDateTime(reader["to_date"]);

                        projectList.Add(project);
                    }
                }
            }
            catch
            {
                return projectList = new List<Project>();
            }
            return projectList;
        }

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool result = false;

            string cmndTxt = "INSERT INTO project_employee (employee_id, project_id) " +
                             "VALUES (@employee_id, @project_id)";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndTxt, sqlConn);
                    sqlCmnd.Parameters.AddWithValue("@employee_id", employeeId);
                    sqlCmnd.Parameters.AddWithValue("@project_id", projectId);
                    int rowsAffected = sqlCmnd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool result = false;

            string cmndTxt = "DELETE FROM project_employee WHERE project_id =" +
                             "@project_id AND employee_id = @employee_id";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndTxt, sqlConn);
                    sqlCmnd.Parameters.AddWithValue("@project_id", projectId);
                    sqlCmnd.Parameters.AddWithValue("@employee_id", employeeId);
                    int rowsAffected = sqlCmnd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }


        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            int newId = 0;

            string cmndText = "INSERT INTO project (name, from_date, to_date) VALUES (@name, @from_date, @to_date)";
            string cmndText2 = "SELECT project_id FROM project WHERE " +
                               "name = @name";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                    sqlCmnd.Parameters.AddWithValue("@name", newProject.Name);
                    sqlCmnd.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    sqlCmnd.Parameters.AddWithValue("@to_date", newProject.EndDate);
                    int rowsAffected = sqlCmnd.ExecuteNonQuery();

                    SqlCommand sqlCmnd2 = new SqlCommand(cmndText2, sqlConn);
                    sqlCmnd2.Parameters.AddWithValue("@name", newProject.Name);
                    SqlDataReader reader = sqlCmnd2.ExecuteReader();

                    if (reader.Read())
                    {
                        newId = Convert.ToInt32(reader["project_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return newId;
            }
            return newId;
        }
    }
}

