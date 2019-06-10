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
    public class ProductTests
    {
        private string dataSource = "Data Source=DESKTOP-TKSEL3D\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestConnectionStringConstructor()
        {
            Product c = new Product(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void TestKeyAndStringConstructor()
        {
            Product c = new Product(1, dataSource);
            Assert.AreEqual("A4CS      ", c.ProductCode);
        }

        [Test]
        public void TestPropAndStringConstructor()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);
            Product c = new Product(props, dataSource);
            Assert.AreEqual("A4CS      ", c.ProductCode);
        }

        [Test]
        public void TestUpdate()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            Product c = new Product(1, dataSource);
            string prodCode = c.ProductCode.ToString();
            c.ProductCode = "zzzz      ";
            c.Save();

            Product d = new Product(1, dataSource);

            Assert.AreEqual(d.ProductCode, c.ProductCode);
        }

        [Test]
        public void TestDelete()
        {
            Product c = new Product(1, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Product(1, dataSource));
        }

        [Test]
        public void TestGetList()
        {
            Product c = new Product(dataSource);
            List<Product> products = (List<Product>)c.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ID);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product p = new Product(dataSource);
            p.ProductCode = "zzzz      ";
            p.Description = "Test";
            p.UnitPrice = 100.00m;
            p.OnHandQuantity = 100;

            p.Save();
            Assert.AreEqual(p.ProductCode, "zzzz      ");
        }

        [Test]
        public void TestRetrieveFromDataStore()
        {
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ID, 1);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "this is a test";
            Assert.Throws<Exception>(() => p.Save());
        }
    }
}
