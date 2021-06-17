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
            
        }
    }
}
