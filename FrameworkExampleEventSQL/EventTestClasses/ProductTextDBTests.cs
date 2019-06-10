using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using EventPropsClassses;
using EventDBClasses;

namespace EventTestClasses
{
    [TestFixture]
    class ProductTextDBTests
    {
        private string folder = "C:\\Courses\\CS234CSharp\\Demos\\FrameworkExampleEvent\\Files\\";

        [Test]
        public void TestProductTextDBRetrieve()
        {
            ProductTextDB db = new ProductTextDB(folder);
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("P100", props.code);
            Assert.AreEqual(10, props.onHandQuantity);
        }

        [Test]
        public void TestProductTextDBCreate()
        {
            ProductTextDB db = new ProductTextDB(folder);
            ProductProps props = new ProductProps();
            props.code = "P300";
            props.description = "This is the description of the third product";
            props.onHandQuantity = 30;
            props.unitPrice = 300M;
            ProductProps props2 = (ProductProps)db.Create(props);
            Console.WriteLine(props.id);
            Console.WriteLine(props2.id);
            Assert.AreEqual(props.id, props2.id);
            Assert.AreEqual(props.concurrencyID, props2.concurrencyID);
            props = (ProductProps)db.Retrieve(3);
            Assert.AreEqual("P300", props.code);
        }

        [Test]
        public void TestProductTextDBDelete()
        {
            ProductTextDB db = new ProductTextDB(folder);
            ProductProps props = (ProductProps)db.Retrieve(1);
            db.Delete(props);
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        [SetUp]
        public void WriteListOfProps()
        {
            List<ProductProps> products = new List<ProductProps>();

            ProductProps props = new ProductProps();
            props.id = 1;
            props.code = "P100";
            props.description = "This is the description of the first product";
            props.onHandQuantity = 10;
            props.unitPrice = 100M;
            products.Add(props);

            props = new ProductProps();
            props.id = 2;
            props.code = "P200";
            props.description = "This is the description of the second product";
            props.onHandQuantity = 20;
            props.unitPrice = 200M;
            products.Add(props);

            XmlSerializer serializer = new XmlSerializer(products.GetType());
            Stream writer = new FileStream(folder + "products.xml", FileMode.Create);
            serializer.Serialize(writer, products);
            writer.Close();
        }
    }
}
