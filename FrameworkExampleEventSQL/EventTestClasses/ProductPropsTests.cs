using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClasses;
using NUnit.Framework;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps props1;

        [SetUp]
        public void SetupAllTests()
        {
            props1 = new ProductProps();

            props1.ProductCode = "zzzz      ";
            props1.Description = "Test";
            props1.UnitPrice = 300.00m;
            props1.OnHandQuantity = 1234;
            props1.ConcurrencyID = 12;
        }

        [Test]
        public void TestClone()
        {
            ProductProps props2 = (ProductProps)props1.Clone();

            Assert.NotNull(props2);
            props1.ProductCode = "zzzz";
            Assert.AreNotEqual(props2.ProductCode, props1.ProductCode);
            Assert.AreNotSame(props2, props1);
        }

        [Test]
        public void TestGetState()
        {
            string xml = props1.GetState();
            Console.WriteLine(xml);
        }

        [Test]
        public void TestSetState()
        {
            string xml = props1.GetState();
            ProductProps props2 = new ProductProps();
            props2.SetState(xml);

            Assert.AreEqual(props1.ProductCode, props2.ProductCode);
            Assert.AreEqual(props1.Description, props2.Description);
        }
    }
}
