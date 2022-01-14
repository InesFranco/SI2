using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrabalhoSI2.concrete;
using TrabalhoSI2.dal;
using TrabalhoSI2.helper;
using TrabalhoSI2.mapper;
using TrabalhoSI2.model;

class Program
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["ex1cs"].ConnectionString;
    private enum Option
    {
        Exit,
        GetTeam,
        CreateIntervention,
        CreateTeam,
        AddTeamElement,
        RemoveTeamElement
    }

    private static void GetTeam()
    {
        using (IContext ctx = new Context(connectionString))
        {
            EquipaMapper mapper = new EquipaMapper(ctx);
            int? id = SQLMapperHelper.ExecuteScalar<int?>(ctx, "select dbo.encontrarEquipaParaIntervencao(@id_intervencao)", new IDbDataParameter[] { new SqlParameter("@id_intervencao", 1) });
            if (id == null)
            {
                Console.WriteLine("There is no team available");
                return;
            }
            
            IEquipa equipa = mapper.Read(id.Value);
            Console.WriteLine("Equipa : " + equipa.codigo_equipa);
            Console.WriteLine("Localização : " + equipa.localizacao);
            Console.WriteLine("Número de Elementos : " + equipa.num_elems);
            Console.WriteLine("Supervisor : " + equipa.id_supervisor);
        }
    }

    private static void CreateIntervention()
    {
        using (IContext ctx = new Context(connectionString))
        {
            SqlCommand cmd = ctx.createCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_criaIntervencao";
            cmd.Parameters.Add(new SqlParameter("@id_activo", 1));
            cmd.Parameters.Add(new SqlParameter("@descricao", "Arranjar tubo"));
            cmd.Parameters.Add(new SqlParameter("@valor", 340));
            cmd.Parameters.Add(new SqlParameter("@data_inicio", new DateTime(2022, 4, 22)));
            cmd.Parameters.Add(new SqlParameter("@data_fim", new DateTime(2022, 4, 25)));

            cmd.ExecuteNonQuery();
        }
    }

    static void Main(string[] args)
    {
        GetTeam();
        /*
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
        */
    }
}