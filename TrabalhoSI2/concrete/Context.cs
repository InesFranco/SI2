using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.dal;

namespace TrabalhoSI2.concrete
{
    internal class Context : IContext
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
            con.Open();
            return con.CreateCommand();
        }

        public void Dispose()
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }

    }
}
