using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventPropsClasses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;


// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = ToolsCSharp.BaseSQLDB;
using DBConnection = System.Data.SqlClient.SqlConnection;
using DBCommand = System.Data.SqlClient.SqlCommand;
using DBParameter = System.Data.SqlClient.SqlParameter;
using DBDataReader = System.Data.SqlClient.SqlDataReader;
using DBDataAdapter = System.Data.SqlClient.SqlDataAdapter;

namespace EventDBClasses
{
    public class ProductSQLDB : DBBase, IReadDB, IWriteDB
    {
        public ProductSQLDB() : base() { }
        public ProductSQLDB(string cnString) : base(cnString) { }
        public ProductSQLDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@Description", SqlDbType.VarChar);
            command.Parameters.Add("@UnitPrice", SqlDbType.Money);
            command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["@ProductID"].Value = props.ID;
            command.Parameters["@ProductCode"].Value = props.ProductCode;
            command.Parameters["@Description"].Value = props.Description;
            command.Parameters["@UnitPrice"].Value = props.UnitPrice;
            command.Parameters["@OnHandQuantity"].Value = props.OnHandQuantity;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.ID;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = (Int32)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }

        public object RetrieveAll(Type type)
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@Description", SqlDbType.VarChar);
            command.Parameters.Add("@UnitPrice", SqlDbType.Money);
            command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.ID;
            command.Parameters["@ProductCode"].Value = props.ProductCode;
            command.Parameters["@Description"].Value = props.Description;
            command.Parameters["@UnitPrice"].Value = props.UnitPrice;
            command.Parameters["@OnHandQuantity"].Value = props.OnHandQuantity;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}
