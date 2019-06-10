using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventClasses;
using EventDBClasses;
using EventPropsClasses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;
using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerTests
    {
        private string dataSource = "Data Source=DESKTOP-TKSEL3D\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestConnectionStringConstructor()
        {
            Customer c = new Customer(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void TestKeyAndStringConstructor()
        {
            Customer c = new Customer(39, dataSource);
            Assert.AreEqual("Fernandes, Philip", c.Name);
        }

        [Test]
        public void TestPropAndStringConstructor()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(39);
            Customer c = new Customer(props, dataSource);
            Assert.AreEqual("Fernandes, Philip", c.Name);
        }

        [Test]
        public void TestUpdate()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            Customer c = new Customer(39, dataSource);
            string name = c.Name.ToString();
            c.Name = "Rogers, Trevor";
            c.Save();

            Customer d = new Customer(39, dataSource);

            Assert.AreEqual(d.Name, c.Name);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(1, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(1, dataSource));
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Trevor Rogers";
            c.Address = "123 Main St";
            c.City = "Eugene";
            c.State = "OR";
            c.Zipcode = "97478";
            c.Save();
            Assert.AreEqual(c.Name, "Trevor Rogers");
        }

        [Test]
        public void TestRetrieveFromDataStore()
        {
            Customer c = new Customer(1, dataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual("Molunguri, A", c.Name);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "this is a test";
            Assert.Throws<Exception>(() => c.Save());
        }
    }
}
