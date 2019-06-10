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
    public class CustomerDBTests
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
        public void TestRetrieve()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(23);
            // 23	Newlin, Sherman	2400 Bel Air, Apt.345	Broomfield	CO	80020          	1
            Assert.AreEqual(23, props.ID);
            Assert.AreEqual("Newlin, Sherman", props.name);
            Assert.AreEqual("2400 Bel Air, Apt.345", props.address);
            Assert.AreEqual("Broomfield", props.city);
            Assert.AreEqual("CO", props.state);
            Assert.AreEqual("80020", props.zipcode);
        }

        [Test]
        public void TestRetrieveAll()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            List<CustomerProps> propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(696, propsList.Count);
        }

        [Test]
        public void TestUpdate()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(23);

            props.name = "zzz";
            props.address = "zzz";
            props.city = "zzz";
            props.state = "OR";
            props.zipcode = "97478";

            db.Update(props);

            CustomerProps propsUpdated = (CustomerProps)db.Retrieve(23);

            Assert.AreEqual("zzz", propsUpdated.name);
            Assert.AreEqual("zzz", propsUpdated.address);
            Assert.AreEqual("zzz", propsUpdated.city);
            Assert.AreEqual("OR", propsUpdated.state);
            Assert.AreEqual("97478", propsUpdated.zipcode);
        }

        [Test]
        public void TestCreate()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = new CustomerProps();

            props.name = "Trevor";
            props.address = "123 Main St";
            props.city = "Eugene";
            props.state = "OR";
            props.zipcode = "97478";

            db.Create(props);

            CustomerProps retCustomer = (CustomerProps)db.Retrieve(props.ID);

            Assert.AreEqual("Trevor", retCustomer.name);
            Assert.AreEqual("123 Main St", retCustomer.address);
            Assert.AreEqual("Eugene", retCustomer.city);
            Assert.AreEqual("OR", retCustomer.state);
            Assert.AreEqual("97478", retCustomer.zipcode);
        }

        [Test]
        public void TestDelete()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = new CustomerProps();

            props.name = "Trevor";
            props.address = "123 Main St";
            props.city = "Eugene";
            props.state = "OR";
            props.zipcode = "97478";

            db.Create(props);

            CustomerProps retCustomer = (CustomerProps)db.Retrieve(props.ID);
            db.Delete(retCustomer);

            Assert.Throws<Exception>(() => db.Retrieve(props.ID));
        }
    }
}
