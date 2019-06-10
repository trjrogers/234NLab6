using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;
using ProductDB = EventDBClasses.ProductSQLDB;
using System.Data;

namespace EventClasses
{
    public class Product : BaseBusiness
    {
        // constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Product() : base()
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
        public Product(string cnString)
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
        public Product(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Product(int key)
            : base(key)
        {
        }

        public Product(ProductProps props)
            : base(props)
        {
        }

        public Product(ProductProps props, string cnString)
            : base(props, cnString)
        {
        }

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop, this.mConnectionString);
                products.Add(p);
            }

            return products;
        }

        public static void Delete(ProductProps p)
        {
            ProductDB db = new ProductDB();
            db.Delete(p);
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("productCode", true);
            mRules.RuleBroken("description", true);
            mRules.RuleBroken("unitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);
        }

        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }

        public int ID
        {
            get
            {
                return ((ProductProps)mProps).ID;
            }
        }

        public string ProductCode
        {
            get
            {
                return ((ProductProps)mProps).ProductCode;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).ProductCode))
                {
                    if (value.Length >= 1 && value.Length <= 10)
                    {
                        mRules.RuleBroken("productCode", false);
                        ((ProductProps)mProps).ProductCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Product code must be between 1 and 10 characters");
                    }
                }
            }
        }
        public string Description
        {
            get
            {
                return ((ProductProps)mProps).Description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).Description))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        mRules.RuleBroken("description", false);
                        ((ProductProps)mProps).Description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Description must be between 1 and 50 characters, inclusive");
                    }
                }
            }
        }
        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).UnitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).UnitPrice))
                {
                    if (value >= 0)
                    {
                        mRules.RuleBroken("unitPrice", false);
                        ((ProductProps)mProps).UnitPrice = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Unite price must be more than 0");
                    }
                }
            }
        }
        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).OnHandQuantity;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).OnHandQuantity))
                {
                    if (value >= 0)
                    {
                        mRules.RuleBroken("onHandQuantity", false);
                        ((ProductProps)mProps).OnHandQuantity = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Onhand quantity must be higher than 0");
                    }
                }
            }
        }
    }
}