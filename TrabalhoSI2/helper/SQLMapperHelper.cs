using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.dal;

namespace TrabalhoSI2.helper
{
    public class SQLMapperHelper
    {

        //Delegate to MAP an entity from a row
        public delegate T Mapper<T>(IDataRecord data);
        public static int ExecuteNonQuery(IContext ctx, string cmdtxt, IDbDataParameter[] dbDataParameters)
        {
            using (SqlCommand cmd = ctx.createCommand())
            {
                cmd.CommandText = cmdtxt;
                cmd.Parameters.AddRange(dbDataParameters);
                ctx.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        

        public static T ExecuteScalar<T>(string connectionString, string cmdtxt, IDbDataParameter[] dbDataParameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdtxt;
                    cmd.Parameters.AddRange(dbDataParameters);
                    con.Open();
                    return (T)cmd.ExecuteScalar();
                }
            }
        }

        public static T ExecuteMapSingle<T>(string connectionString, string cmdtxt, IDbDataParameter[] dbDataParameters, Mapper<T> map)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdtxt;
                    cmd.Parameters.AddRange(dbDataParameters);
                    con.Open();
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            return map((IDataRecord)rd);

                        }
                    }
                }
            }
            return default;
        }

        public static TCol ExecuteMapSet<T, TCol>(string connectionString, string cmdtxt, IDbDataParameter[] dbDataParameters, Mapper<T> map)
            where TCol : class, IList, new()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        }
    }
}
