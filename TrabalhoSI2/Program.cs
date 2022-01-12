using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrabalhoSI2.concrete;
using TrabalhoSI2.dal;
using TrabalhoSI2.helper;

class Program
{
    static void Main(string[] args)
    {
        #region CONFIGURATION
        //Ver ficheiro app.config e alterar a ConnectionString de acordo
        string connectionString = ConfigurationManager.ConnectionStrings["ex1cs"].ConnectionString;
        #endregion

        #region E
        using (IContext ctx = new Context(connectionString))
        {
            int? id = SQLMapperHelper.ExecuteScalar<int>(ctx, "select dbo.encontrarEquipaParaIntervencao(@id_intervencao)", new IDbDataParameter[] { new SqlParameter("@id_intervencao", 1) });
            Console.WriteLine("Equipa " + id + " tem a capacidade de arranjar piscinas");
        }
        #endregion

        #region F
        using (IContext ctx = new Context(connectionString))
        {
            SqlCommand cmd = ctx.createCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_criaIntervencao";
            cmd.Parameters.Add(new SqlParameter("@id_activo", 1));
            cmd.Parameters.Add(new SqlParameter("@descricao", "Arranjar tubo"));
            cmd.Parameters.Add(new SqlParameter("@valor", 340));
            cmd.Parameters.Add(new SqlParameter("@data_inicio", new DateTime(2022,4,22 )));
            cmd.Parameters.Add(new SqlParameter("@data_fim", new DateTime(2022,4,25)));

            cmd.ExecuteNonQuery();
        }
        #endregion

        #region criarEquipa g
        using (IContext ctx = new Context(connectionString))
        {
            SqlCommand cmd = ctx.createCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_criaEquipa";
            cmd.Parameters.Add(new SqlParameter("@localizacao", "Parque eduardo setimo"));
            cmd.Parameters.Add(new SqlParameter("@id_supervisor", 1));
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region (adicionar ou remover) os elementos de uma equipe
        using (IContext ctx = new Context(connectionString))
        {
            SqlCommand cmd = ctx.createCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_criaEquipa";
            cmd.Parameters.Add(new SqlParameter("@localizacao", "Parque eduardo setimo"));
            cmd.Parameters.Add(new SqlParameter("@id_supervisor", 1));
            cmd.ExecuteNonQuery();
        }
        #endregion

    }
}