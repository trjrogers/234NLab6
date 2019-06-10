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
    public class CustomersPropsTests
    {
        CustomerProps props1;

        [SetUp]
        public void SetupAllTests()
        {
            props1 = new CustomerProps();
        
            // Set different field values
            props1.ID = 1;
            props1.name = "Mickey";
            props1.address = "Main Street";
            props1.city = "Orlando";
            props1.state = "FL";
            props1.zipcode = "11111";
            props1.ConcurrencyID = 12;
        }

        [Test]
        public void TestClone()
        {
            // Clones props1 and casts it to CustomerProps type
            CustomerProps props2 = (CustomerProps)props1.Clone();

            Assert.NotNull(props2);
            props1.name = "Minnie";
            Assert.AreNotEqual(props2.name, props1.name);
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
            CustomerProps props2 = new CustomerProps();
            props2.SetState(xml);

            Assert.AreEqual(props1.name, props2.name);
            Assert.AreEqual(props1.address, props2.address);
        }
    }
}
