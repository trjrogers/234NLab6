using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using EventPropsClasses;
using EventDBClasses;

using DBCommand = System.Data.SqlClient.SqlCommand;
using System.Data;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductDBTests
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
        public void TestRetrieve()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);
            //1   A4CS Murach's ASP.NET 4 Web Programming with C# 2010	56.5000	4637	1
            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("A4CS", props.ProductCode.Trim());
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", props.Description);
            Assert.AreEqual(56.5000, props.UnitPrice);
            Assert.AreEqual(4637, props.OnHandQuantity);
        }

        [Test]
        public void TestRetrieveAll()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            List<ProductProps> propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(16, propsList.Count);
        }

        [Test]
        public void TestUpdate()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);

            props.ProductCode = "zzz       ";
            props.Description = "zzz";
            props.UnitPrice = 120.00m;
            props.OnHandQuantity = 1234;

            db.Update(props);

            ProductProps propsUpdated = (ProductProps)db.Retrieve(1);

            Assert.AreEqual("zzz       ", propsUpdated.ProductCode);
            Assert.AreEqual("zzz", propsUpdated.Description);
            Assert.AreEqual(120.00m, propsUpdated.UnitPrice);
            Assert.AreEqual(1234, propsUpdated.OnHandQuantity);
        }

        [Test]
        public void TestCreate()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = new ProductProps();

            props.ProductCode = "zzz       ";
            props.Description = "zzz";
            props.UnitPrice = 120.00m;
            props.OnHandQuantity = 1234;

            db.Create(props);

            ProductProps createdProduct = (ProductProps)db.Retrieve(props.ID);

            Assert.AreEqual("zzz       ", createdProduct.ProductCode);
            Assert.AreEqual("zzz", createdProduct.Description);
            Assert.AreEqual(120.00m, createdProduct.UnitPrice);
            Assert.AreEqual(1234, createdProduct.OnHandQuantity);
        }

        [Test]
        public void TestDelete()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = new ProductProps();

            props.ProductCode = "zzz       ";
            props.Description = "zzz";
            props.UnitPrice = 120.00m;
            props.OnHandQuantity = 1234;

            db.Create(props);

            ProductProps createdProduct = (ProductProps)db.Retrieve(props.ID);
            db.Delete(createdProduct);

            Assert.Throws<Exception>(() => db.Retrieve(props.ID));
        }
    }
}
