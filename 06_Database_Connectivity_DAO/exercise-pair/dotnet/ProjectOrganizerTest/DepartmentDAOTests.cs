using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace ProjectOrganizerTest
{
    [TestClass]
    public class DepartmentDAOTests : DAOTests
    {
        [TestMethod]
        public void DeptDAOConstructor()
        {
            Assert.IsNotNull(deptDAO);
        }

        [TestMethod]
        public void GetDepartmentsSELECT()
        {
            int rowCount = GetRowCount("department");

            IList<Department> deptList = deptDAO.GetDepartments();

            Assert.AreEqual(rowCount, deptList.Count);
        }

        [TestMethod]
        public void CreateDepartmentINSERT()
        {
            int rowCount = GetRowCount("department");

            deptDAO.CreateDepartment(department);

            int rowCountAfter = GetRowCount("department");

            Assert.AreEqual(rowCount + 1, rowCountAfter);
        }

        [TestMethod]
        public void UpdateDepartmentUPDATE()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string cmndText = "SELECT name FROM department WHERE department_id = " +
                                  "(SELECT MAX(department_id) FROM department)";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
                string result = Convert.ToString(sqlCmnd.ExecuteScalar());

                deptDAO.UpdateDepartment(department);

                SqlCommand sqlCmnd2 = new SqlCommand(cmndText, conn);
                string resultAfter = Convert.ToString(sqlCmnd.ExecuteScalar());

                Assert.AreNotEqual(result, resultAfter);
            }
        }
    }
}
