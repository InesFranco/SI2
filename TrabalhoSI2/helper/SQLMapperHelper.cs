using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.concrete;
using TrabalhoSI2.dal;

namespace TrabalhoSI2.helper
{
    public class SQLMapperHelper
    {

        //Delegate to MAP an entity from a row
        public delegate T Mapper<T>(IDataRecord data);
        public static int ExecuteNonQuery(IContext ctx, CommandType commandType, string cmdtxt, IDbDataParameter[] dbDataParameters)
        {
            try
            {
                using (SqlCommand cmd = ctx.createCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = cmdtxt;
                    cmd.Parameters.AddRange(dbDataParameters);
                    return cmd.ExecuteNonQuery();
                }
            }catch(Exception ex)
            {
                throw(ex);
            }
            
        }
        

        public static T? ExecuteScalar<T>(IContext ctx, CommandType commandType, string cmdtxt, IDbDataParameter[] dbDataParameters)
        {
            try
            {
                using (SqlCommand cmd = ctx.createCommand())
                {

                    cmd.CommandType = commandType;
                    cmd.CommandText = cmdtxt;
                    cmd.Parameters.AddRange(dbDataParameters);
                    object result = cmd.ExecuteScalar();
                    if(!Convert.IsDBNull(result))
                    {
                        return (T)result;
                    }
                    return default;
                }
            }catch(SqlException ex)
            {
                throw;
            }
            
        }

        public static T ExecuteMapSingle<T>(IContext ctx, string cmdtxt, IDbDataParameter[] dbDataParameters, Mapper<T> map)
        {
           using (SqlCommand cmd = ctx.createCommand())
            {
                cmd.CommandText = cmdtxt;
                cmd.Parameters.AddRange(dbDataParameters);
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return map((IDataRecord)rd);
                    }
                }
            }
            return default;
        }

        public static TCol ExecuteMapSet<T, TCol>(IContext ctx, string cmdtxt, IDbDataParameter[] dbDataParameters, Mapper<T> map)
            where TCol : class, IList, new()
        {
            using (SqlCommand cmd = ctx.createCommand())
            {
                cmd.CommandText = cmdtxt;
                cmd.Parameters.AddRange(dbDataParameters);

                IList<T> list = new List<T>();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(map((IDataRecord)rd));
                    }
                }
                return (TCol)list;
            }

        }

        
    }
}

