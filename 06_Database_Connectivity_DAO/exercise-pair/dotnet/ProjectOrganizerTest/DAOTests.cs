using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace ProjectOrganizerTest
{
    [TestClass]
    public class DAOTests
    {
        protected string connectionString = "Data Source=.\\sqlexpress;" +
                                                   "Initial Catalog=EmployeeDB;" +
                                                   "Integrated Security=True";

        protected DepartmentSqlDAO deptDAO;
        protected EmployeeSqlDAO emplDAO;
        protected ProjectSqlDAO projDAO;

        public DAOTests()
        {
            deptDAO = new DepartmentSqlDAO(connectionString);
            emplDAO = new EmployeeSqlDAO(connectionString);
            projDAO = new ProjectSqlDAO(connectionString);
        }


        protected Department department;
        protected Employee employee;
        protected int wilEmplId = 0;
        protected Project project;

        //private TransactionScope tran;


        [TestInitialize]
        public void Setup()
        {
            //tran = new TransactionScope();

            department = new Department()
            { Name = "Matters of Great Import" };

            employee = new Employee()
            {
                FirstName = "Wil",
                LastName = "Whipple",
                BirthDate = Convert.ToDateTime("1976-02-19"),
                HireDate = Convert.ToDateTime("1990-01-01"),
                JobTitle = "Grand Czar"
            };

            project = new Project()
            {
                Name = "godsLove",
                StartDate = Convert.ToDateTime("2021-07-02"),
                EndDate = Convert.ToDateTime("2021-08-16")
            };
        }


        [TestCleanup]
        public void Cleanup()
        {
            //tran.Dispose();
        }

        public int AddNewEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string cmndTxt = "INSERT INTO employee (department_id, first_name, last_name," +
                                  " job_title, birth_date, hire_date)" +
                                  " VALUES ((SELECT MIN(department_id) FROM department), " +
                                  "'Wil', 'Whipple', 'Grand Czar', '1976-02-19', '1990-01-01');" +
                                  " SELECT scope_identity()";

                SqlCommand sqlCmnd = new SqlCommand(cmndTxt, conn);
                wilEmplId = Convert.ToInt32(sqlCmnd.ExecuteScalar());
            }
            return wilEmplId;
        }

        public int AddNewDeptartment()
        {
            int newDeptId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string cmndTxt = "INSERT INTO department (name) " +
                                 "VALUES ('Matters of Great Import')";

                SqlCommand sqlCmnd = new SqlCommand(cmndTxt, conn);
                newDeptId = Convert.ToInt32(sqlCmnd.ExecuteScalar());
            }
            return newDeptId;
        }

        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }
    }
}
