using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;
using CustomerDB = EventDBClasses.CustomerSQLDB;
using System.Data;

namespace EventClasses
{
    public class Customer : BaseBusiness
    {
        // constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Customer() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Customer(int key)
            : base(key)
        {
        }

        public Customer(CustomerProps props)
            : base(props)
        {
        }

        public Customer(CustomerProps props, string cnString)
            : base(props, cnString)
        {
        }

        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop, this.mConnectionString);
                customers.Add(c);
            }

            return customers;
        }

        public static void Delete(CustomerProps c)
        {
            CustomerDB db = new CustomerDB();
            db.Delete(c);
        }
	
        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("name", true);
            mRules.RuleBroken("address", true);
            mRules.RuleBroken("city", true);
            mRules.RuleBroken("state", true);
            mRules.RuleBroken("zipcode", true);
        }

        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new CustomerDB();
                mdbWriteable = new CustomerDB();
            }

            else
            {
                mdbReadable = new CustomerDB(this.mConnectionString);
                mdbWriteable = new CustomerDB(this.mConnectionString);
            }
        }

        public int ID
        {
            get
            {
                return ((CustomerProps)mProps).ID;
            }
        }

        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).name))
                {
                    if (value.Length >= 1 && value.Length <= 100)
                    {
                        mRules.RuleBroken("name", false);
                        ((CustomerProps)mProps).name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 100 characters, inclusive");
                    }
                }
            }
        }
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).address))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        mRules.RuleBroken("address", false);
                        ((CustomerProps)mProps).address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Address must be between 1 and 50 characters, inclusive");
                    }
                }
            }
        }
        public string City
        {
            get
            {
                return ((CustomerProps)mProps).city;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).city))
                {
                    if (value.Length >= 1 && value.Length <= 20)
                    {
                        mRules.RuleBroken("city", false);
                        ((CustomerProps)mProps).city = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 20 characters, inclusive");
                    }
                }
            }
        }
        public string State
        {
            get
            {
                return ((CustomerProps)mProps).state;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).state))
                {
                    if (value.Length == 2)
                    {
                        mRules.RuleBroken("state", false);
                        ((CustomerProps)mProps).state = value.ToUpper();
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("State must be 2 characters");
                    }
                }
            }
        }
        public string Zipcode
        {
            get
            {
                return ((CustomerProps)mProps).zipcode;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).zipcode))
                {
                    if (value.Length >= 1 && value.Length <= 15)
                    {
                        mRules.RuleBroken("zipcode", false);
                        ((CustomerProps)mProps).zipcode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Zip Code must be between 1 and 15 characters, inclusive");
                    }
                }
            }
        }
    }
}