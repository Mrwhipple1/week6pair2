using System;
using System.Collections.Generic;
using System.Text;
using ProjectOrganizer.DAL;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.Models;
using System.Data.SqlClient;

namespace ProjectOrganizerTest
{
    [TestClass]
    public class EmployeeDAOTests : DAOTests
    {
        [TestMethod]
        public void EmplDAOConstructor()
        {
            Assert.IsNotNull(emplDAO);
        }

        [TestMethod]
        public void GetAllEmployeesSELECT()
        {
            int rowCount = GetRowCount("employee");

            IList<Employee> emplList = emplDAO.GetAllEmployees();

            Assert.AreEqual(rowCount, emplList.Count);
        }

        [TestMethod]
        public void SearchSELECT()
        {
            DAOTests newEmpl = new DAOTests();
            int newEmplId = newEmpl.AddNewEmployee();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                emplDAO.Search(employee.FirstName, employee.LastName);

                string cmndText2 = "SELECT employee_id FROM employee WHERE " +
                                  "first_name = @first_name AND last_name = @last_name";

                SqlCommand sqlCmnd = new SqlCommand(cmndText2, conn);
                sqlCmnd.Parameters.AddWithValue("@first_name", employee.FirstName);
                sqlCmnd.Parameters.AddWithValue("@last_name", employee.LastName);
                int returnedId = Convert.ToInt32(sqlCmnd.ExecuteScalar());

                Assert.AreEqual(newEmplId, returnedId);
            }
        }


        [TestMethod]
        public void GetEmployeesWithoutProjectsSELECT()
        {

        }
    }
}
