using System;
using System.Collections.Generic;
using System.Text;
using ProjectOrganizer.DAL;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using ProjectOrganizer.Models;

namespace ProjectOrganizerTest
{
    [TestClass]
    public class ProjectDAOTests : DAOTests
    {
        [TestMethod]
        public void ProjDAOConstructor()
        {
            Assert.IsNotNull(projDAO);
        }

        [TestMethod]
        //[DataRow("project_id, name, from_date, to_date FROM project")]?
        public void GetAllProjectsSELECT()
        {
            int rowCount = GetRowCount("project");

            IList<Project> projList = projDAO.GetAllProjects();

            Assert.AreEqual(rowCount, projList.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProjectINSERT()
        {
            DAOTests newEmpl = new DAOTests();
            wilEmplId = newEmpl.AddNewEmployee();

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                int rowCount = GetRowCount("project_employee");

                string cmndText = "SELECT MIN(project_id) FROM project";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                int projForWil = Convert.ToInt32(sqlCmnd.ExecuteScalar());

                projDAO.AssignEmployeeToProject(projForWil, wilEmplId);

                int rowCountAfter = GetRowCount("project_employee");

                Assert.AreEqual(rowCount + 1, rowCountAfter);
            }
        }

            [TestMethod]
        public void RemoveEmployeeFromProjectDELETE()
        {
            DAOTests newEmpl = new DAOTests();
            wilEmplId = newEmpl.AddNewEmployee();

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                string cmndText = "SELECT MIN(project_id) FROM project";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, sqlConn);
                int projForWil = Convert.ToInt32(sqlCmnd.ExecuteScalar());

                projDAO.AssignEmployeeToProject(projForWil, wilEmplId);

                int rowCount = GetRowCount("project_employee");

                projDAO.RemoveEmployeeFromProject(projForWil, wilEmplId);

                int rowCountAfter = GetRowCount("project_employee");

                Assert.AreEqual(rowCount - 1, rowCountAfter);
            }
        }

        [TestMethod]
        public void CreateProjectINSERT()
        {
            int rowCount = GetRowCount("project");

            projDAO.CreateProject(project);

            int rowCountAfter = GetRowCount("project");

            Assert.AreEqual(rowCount + 1, rowCountAfter);
        }
    }
}