using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TrabalhoSI2.dal;

namespace TrabalhoSI2.concrete
{
    public class Context : IContext
    {
        private string connectionString;
        private SqlConnection con = null;
        public Context(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Open()
        {
            if (con == null)
            {
                con = new SqlConnection(connectionString);
            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        public SqlCommand createCommand()
        {
            Open();
            return con.CreateCommand();
        }

        public void Dispose()
        {
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
        }

        public void EnlistTransaction()
        {
            if (con != null)
            {
                con.EnlistTransaction(Transaction.Current);
            }
        }

    }
}
