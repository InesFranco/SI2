using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

    private static void printTeams(int top)
    {
        using (IContext ctx = new Context(connectionString))
        {
            EquipaMapper mapper = new EquipaMapper(ctx);

            IList<IEquipa> equipas = mapper.ReadAll(top);

            foreach (Equipa e in equipas)
            {
                Console.Write("Código da Equipa: " + e.codigo_equipa + "- Localização: " + e.localizacao +
                    "- num_elems: " + e.num_elems + "- id supervisor: " + e.id_supervisor);
                Console.WriteLine();
            }
        }
    }

    private static void GetTeamWithQualifications()
    {
        using (IContext ctx = new Context(connectionString))
        {
            printTeams(10);
            EquipaMapper mapper = new EquipaMapper(ctx);
            
            int intervencaoId = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("Insere o id da intervenção: ");
                     intervencaoId = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Valor Inválido");
                }
            }
            
            

            int? id = SQLMapperHelper.ExecuteScalar<int?>(ctx, CommandType.Text, "select dbo.encontrarEquipaParaIntervencao(@id_intervencao)", new IDbDataParameter[] { new SqlParameter("@id_intervencao", intervencaoId) });
            if (id == null)
            {
                Console.WriteLine("Não existe nenhuma equipa com essas capacidades/disponivel");
                return;
            }

            
            IEquipa equipa = mapper.Read(id.Value);
            Console.WriteLine("Equipa : " + equipa.codigo_equipa);
            Console.WriteLine("Localização : " + equipa.localizacao);
            Console.WriteLine("Número de Elementos : " + equipa.num_elems);
            Console.WriteLine("Supervisor : " + equipa.id_supervisor);
            foreach(Funcionario funcionario in equipa.TeamMembers)
            {
                Console.WriteLine("Funcionario: " + funcionario.nome + " profissão : " + funcionario.profissao);
            }

        }
    }


    private static void CreateIntervention()
    {
        int id_activo = -1;
        string descricao = "";
        int valor = -1;
        DateTime dataInicio = new DateTime();
        DateTime dataFim = new DateTime();

        var cultureInfo = new CultureInfo("pt-PT");
        
        while (true)
        {
            try
            {
                Console.WriteLine("Voltar atrás?(y)");
                if (Console.ReadLine().Equals('y')) return;


                Console.WriteLine("Qual é o Id do activo?: ");
                id_activo = int.Parse(Console.ReadLine());

                Console.WriteLine("Qual é a descrição da intervenção?: ");
                descricao = Console.ReadLine();

                Console.WriteLine("Qual o valor da intervenção?: ");
                valor = int.Parse(Console.ReadLine());

                Console.WriteLine("Qual a data de inicio?(dd-mm-aaaa): ");
                dataInicio = DateTime.Parse(Console.ReadLine(), cultureInfo,
                                        DateTimeStyles.NoCurrentDateDefault);

                Console.WriteLine("Qual a data de fim?(dd-mm-aaaa): ");
                dataFim = DateTime.Parse(Console.ReadLine(), cultureInfo,
                                        DateTimeStyles.NoCurrentDateDefault);
                break;
            }catch(Exception ex)
            {
                Console.WriteLine("Valor Inválido");
                continue;
            }
            
        }
        try
        {
            using (IContext ctx = new Context(connectionString))
            {
                IntervencaoMapper intervencaoMapper = new IntervencaoMapper(ctx);

                Intervencao intervencao = new Intervencao();
                intervencao.id_activo = id_activo;
                intervencao.descricao = descricao;
                intervencao.valor = valor;
                intervencao.dataInicio = dataInicio;
                intervencao.dataFim = dataFim;

                intervencaoMapper.Create(intervencao);
                if(intervencao.id_intervencao != null)
                    intervencaoMapper.Read((int)intervencao.id_intervencao);
                    
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }


    private static void CreateTeam()
    {
        string localizacao = null;
        int idSupervisor = -1;

        while (true)
        {
            try
            {
                Console.WriteLine("Voltar atrás?(y)");
                if (Console.ReadLine().Equals('y')) return;


                Console.WriteLine("Localização da equipa?: ");
                localizacao = Console.ReadLine();

                Console.WriteLine("Id do Supervisor?: ");
                idSupervisor = int.Parse(Console.ReadLine());

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Valor Inválido");
                continue;
            }
        }

        using (IContext ctx = new Context(connectionString))
        {
            IEquipaMapper equipaMapper = new EquipaMapper(ctx);
            IEquipa equipa = new Equipa();
            equipa.localizacao = localizacao;
            equipa.id_supervisor = idSupervisor;
            equipa = equipaMapper.Create(equipa);
        }
    }

    private static void printIntervencoesInYear()
    {
        int year = -1;
        while (true)
        {
            Console.WriteLine("Produzir Listagem de Intervencao de que ano?");
            try
            {
                year = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Valor Inválido");
            }
        }
    

        IList<Intervencao> intervenctions = new List<Intervencao>();
        using (IContext ctx = new Context(connectionString))
        {
            IntervencaoMapper intervencaoMapper = new IntervencaoMapper(ctx);
            intervenctions = intervencaoMapper.ReadAllYear(year);
        }

        Console.WriteLine("Intervenções:");
        foreach (Intervencao intervencao in intervenctions)
        {
            Console.WriteLine("Id: " + intervencao.id_intervencao + ", Descrição: " + intervencao.descricao);
        }
            
    }


    private static void AddElementToTeam()
    {
        using (IContext ctx = new Context(connectionString))
        {
            printTeams(10);
            
            Equipa equipa = new Equipa();
            EquipaMapper equipaMapper = new EquipaMapper(ctx);

            int idFuncionario = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("Acrescentar Elementos a que equipa?");
                    equipa.codigo_equipa = int.Parse(Console.ReadLine());

                    Console.WriteLine("Qual o id do Funcionário?");
                    idFuncionario = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Valor Inválido");
                }
            }
                    
            equipa = (Equipa)equipaMapper.UpdateAddTeamMembers(equipa, idFuncionario);

            foreach (Funcionario funcionario in equipa.TeamMembers)
            {
                Console.WriteLine("Membro: " + funcionario.nome + " profissão : " + funcionario.profissao);
            }
        }
    }


    private static void RemoveElementInTeam()
    {
        using (IContext ctx = new Context(connectionString))
        {
            printTeams(10);

            Equipa equipa = new Equipa();
            IEquipaMapper equipaMapper = new EquipaMapper(ctx);

            int idFuncionario = - 1;

            while (true)
            {
                try
                {
                    Console.WriteLine("Retirar Elemento de que equipa?");
                    equipa.codigo_equipa = int.Parse(Console.ReadLine());

                    Console.WriteLine("Qual o id do Funcionário?");
                    idFuncionario = int.Parse(Console.ReadLine());
                    break;

                }catch(Exception ex)
                {
                    Console.WriteLine("Valor Inválido");
                }
            }

            equipa = (Equipa)equipaMapper.Read(equipa.codigo_equipa);
            equipa = (Equipa)equipaMapper.UpdateRemoveTeamMember(equipa, idFuncionario);

            foreach (Funcionario funcionario in equipa.TeamMembers)
            {
                Console.WriteLine("Membro: " + funcionario.nome + " profissão : " + funcionario.profissao);
            }


        }
    }

    static void Main(string[] args)
    {
        //GetTeamWithQualifications();
        //CreateIntervention();
        //CreateTeam();
        //AddElementToTeam();
        RemoveElementInTeam();
        //printIntervencoesInYear();
        //TODO: associar as respectivas competˆencias;
        //TODO: sem procedimentos (f) Criar o procedimento p criaInter que permite criar uma interven¸c˜ao;

    }
}