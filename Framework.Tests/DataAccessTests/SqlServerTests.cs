using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Framework.Tests.DataAccessTests
{
    [Ignore("Testing")]
    [TestFixture]
    public class SqlServerTests
    {
        [OneTimeSetUp]
        public void CreateDb()
        {
            //TODO: Connect to database and confirm permissions
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString());
            using (SqlCommand command = con.CreateCommand())
            {
                con.Open();
                command.CommandText = "CREATE DATABASE FrameworkTests";
                command.ExecuteNonQuery();
            }
    
            //TODO: Generate tables and test store procedures

            //TODO: Act

            //TODO: Assert
        }

        [OneTimeTearDown]
        public void DeleteDb()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString());
            using (SqlCommand command = con.CreateCommand())
            {
                con.Open();
                command.CommandText = "DROP DATABASE FrameworkTests";
                command.ExecuteNonQuery();
            }
        }

        [Test]
        public void FakeTest()
        {
            Assert.True(true);
        }
    }
}