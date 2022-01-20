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

    private static void printIntervention(Intervencao intervencao)
    {
        Console.WriteLine();
        Console.WriteLine("id intervenção: " + intervencao.id_intervencao);
        Console.WriteLine("id activo: " + intervencao.id_activo);
        Console.WriteLine("descrição: " + intervencao.descricao);
        Console.WriteLine("estado: " + intervencao.estado);
        Console.WriteLine("valor : " + intervencao.valor);
        Console.WriteLine("data inicio: " + intervencao.dataInicio);
        Console.WriteLine("data fim : " + intervencao.dataFim);
        Console.WriteLine();
    }

    private static void printTeam(Equipa equipa)
    {
        Console.WriteLine();
        Console.WriteLine("id Equipa: " + equipa.codigo_equipa);
        Console.WriteLine("Localização: " + equipa.localizacao);
        Console.WriteLine("Número de Elementos: " + equipa.num_elems);
        Console.WriteLine("id Supervisor: " + equipa.id_supervisor);
        foreach(Funcionario member in equipa.TeamMembers)
        {
            Console.WriteLine("Membro: " + member.nome);
            Console.WriteLine("Email: " + member.email);
            Console.WriteLine("Endereço: " + member.endereco);
        }
        Console.WriteLine();
    }

    private static Equipa GetTeamWithQualifications()
    {
        using (IContext ctx = new Context(connectionString))
        {
            EquipaMapper equipaMapper = new EquipaMapper(ctx);
            
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

            EquipaProxy equipa = (EquipaProxy)equipaMapper.ReadTeamWithQualifications(intervencaoId);
            if (equipa == null)
            {
                Console.WriteLine("Não existe nenhuma equipa com essas capacidades/disponivel");
                return null;
            }
                
            Console.WriteLine("Equipa : " + equipa.codigo_equipa);
            Console.WriteLine("Localização : " + equipa.localizacao);
            Console.WriteLine("Número de Elementos : " + equipa.num_elems);
            Console.WriteLine("Supervisor : " + equipa.id_supervisor);
            foreach(Funcionario funcionario in equipa.TeamMembers)
            {
                Console.WriteLine("Funcionario: " + funcionario.nome + " profissão : " + funcionario.profissao);
            }
            Console.WriteLine();
            return equipa;
        }
    }


    private static void CreateIntervention()
    {
        int id_activo;
        string descricao;
        int valor;
        DateTime dataInicio;
        DateTime dataFim;

        var cultureInfo = new CultureInfo("pt-PT");
        
        while (true)
        {
            try
            {
                Console.WriteLine("Voltar atrás?(y)");
                if (Console.ReadLine().Equals("y")) return;


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

                intervencao = intervencaoMapper.Create(intervencao);

                if(intervencao.id_intervencao != null)
                {
                    intervencao = intervencaoMapper.Read((int)intervencao.id_intervencao);
                    printIntervention(intervencao);
                }
            }
        }catch(SqlException ex)
        {
            // Foreign Key violation
            if (ex.Number == 547)
            {
                Console.WriteLine("O id do activo não existe. Tenta outra vez!");
            }
            else
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        
    }


    private static void CreateTeam()
    {
        string localizacao;
        int idSupervisor;

        while (true)
        {
            try
            {
                Console.WriteLine("Voltar atrás?(y)");
                if (Console.ReadLine().Equals("y")) return;


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

        try
        {
            using (IContext ctx = new Context(connectionString))
            {
                IEquipaMapper equipaMapper = new EquipaMapper(ctx);
                IEquipa equipa = new Equipa();
                equipa.localizacao = localizacao;
                equipa.id_supervisor = idSupervisor;
                equipa = equipaMapper.Create(equipa);
                equipa = equipaMapper.Read(equipa.codigo_equipa);
                printTeam((Equipa)equipa);
            }
        }catch(SqlException ex)
        {
            // Foreign Key violation
            if (ex.Number == 547)
            {
                Console.WriteLine("O id do supervisor não existe. Tenta outra vez!");
            }
            else
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }

    private static void printIntervencoesInYear()
    {
        int year;
        while (true)
        {
            Console.WriteLine("Produzir Listagem de Intervencao de que ano?");
            try
            {
                year = int.Parse(Console.ReadLine());
                break;
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

        if(intervenctions.Count() != 0)
        {
            Console.WriteLine("Intervenções:");
            foreach (Intervencao intervencao in intervenctions)
            {
                Console.WriteLine("Id: " + intervencao.id_intervencao + ", Descrição: " + intervencao.descricao);
            }
        }
        else
        {
            Console.WriteLine("Não houve intervenções nesse ano!");
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
            try
            {
                equipa = (Equipa)equipaMapper.UpdateAddTeamMembers(equipa, idFuncionario);
            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            

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

            IEquipaMapper equipaMapper = new EquipaMapper(ctx);

            int idFuncionario = - 1;
            Equipa equipa = new Equipa();

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

            try
            {
                
                equipa = (Equipa)equipaMapper.Read(equipa.codigo_equipa);
                equipa = (Equipa)equipaMapper.UpdateRemoveTeamMember(equipa, idFuncionario);
                foreach (Funcionario funcionario in equipa.TeamMembers)
                {
                    Console.WriteLine("Membro: " + funcionario.nome + " profissão : " + funcionario.profissao);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static void AddCompetencias()
    {
        int idCompetencia = -1;
        int idFuncionario = -1;
        while (true)
        {
            try
            {
                Console.WriteLine("Qual o id do funcionário?");
                idFuncionario = int.Parse(Console.ReadLine());

                Console.WriteLine("Qual o id da competência?");
                idCompetencia = int.Parse(Console.ReadLine());
                break;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Valor Inválido");
            }
        }

        using (Context ctx = new Context(connectionString))
        {
            try
            {
                FuncionarioMapper funcionarioMapper = new FuncionarioMapper(ctx);
                Funcionario funcionario = funcionarioMapper.Read(idFuncionario);

                funcionarioMapper.AddCompetencia(funcionario, idCompetencia);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }

    private static void CreateInterventionNoProcedure()
    {
        int id_activo;
        string descricao;
        int valor;
        DateTime dataInicio;
        DateTime dataFim;

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
                if (dataInicio > dataFim)
                {
                    Console.WriteLine("Data de inicio superior a data de fim");
                }
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
            DateTime dataAquisicao = SQLMapperHelper.ExecuteScalar<DateTime>(ctx, CommandType.Text, "select data_aquisicao from activo where activo_id=@activo_id",
                new IDbDataParameter[]
                {
                    new SqlParameter("@activo_id", id_activo)
                });

            if(dataAquisicao > dataInicio)
            {
                Console.WriteLine("Activo obtido depois da data de inicio da intervenção");
                return;
            }

            Intervencao intervencao = new Intervencao();
            intervencao.valor = valor;
            intervencao.id_activo = id_activo;
            intervencao.estado = "por atribuir";
            intervencao.descricao = descricao;
            intervencao.dataInicio = dataInicio;
            intervencao.dataFim = dataFim;

            IntervencaoMapper intervencaoMapper = new IntervencaoMapper(ctx);
            try
            {
                intervencao = intervencaoMapper.CreateNoProcedure(intervencao);

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (intervencao.id_intervencao != null)
            {
                intervencao = intervencaoMapper.Read((int)intervencao.id_intervencao);

                Console.WriteLine("id intervenção: " + intervencao.id_intervencao + " id activo " + intervencao.id_activo + " descrição: " + intervencao.descricao
                            + " estado: " + intervencao.estado + " valor : " + intervencao.estado + " data inicio: " + intervencao.dataInicio + " data fim : " + intervencao.dataFim);
            }
            
        }
    }

    private static void AttributeAnIntervention()
    {
        int id_activo;
        string descricao;
        string estado;
        float valor;
        DateTime dataInicio;
        DateTime dataFim ;

        var cultureInfo = new CultureInfo("pt-PT");

        while (true)
        {
            try
            {
                Console.WriteLine("Voltar atrás?(y)");
                if (Console.ReadLine().Equals('y')) return;


                Console.WriteLine("Qual é o Id do activo?: ");
                id_activo = int.Parse(Console.ReadLine());

                Console.WriteLine("Qual é o Estado da Intervencao?: ");
                estado = Console.ReadLine();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Valor Inválido");
                continue;
            }

        }
        Intervencao intervencao = new Intervencao();
        intervencao.id_activo = id_activo;
        intervencao.descricao = descricao;
        intervencao.valor = valor;
        intervencao.dataInicio = dataInicio;
        intervencao.dataFim = dataFim;

        using (Context ctx = new Context(connectionString))
        {
            //Create intervention
            IntervencaoMapper intervencaoMapper = new IntervencaoMapper(ctx);
            intervencao = intervencaoMapper.Create(intervencao);
            intervencao.estado = estado;
            intervencaoMapper.Update(intervencao);      //update intervention status

            Console.WriteLine("id intervenção : " + intervencao.id_intervencao + " descrição: " + intervencao.descricao);

            //Assign a team
            EquipaMapper equipaMapper = new EquipaMapper(ctx);
            IEquipa equipa = GetTeamWithQualifications();
            if(equipa != null)
            {
                equipa = equipaMapper.AssignIntervention(equipa, intervencao);

                Console.WriteLine("Equipa: " + equipa.codigo_equipa);
                Console.WriteLine("Intervenções:");
                foreach (Intervencao i in equipa.Intervencoes)
                {
                    Console.WriteLine("Id da Intervenção: " + i.id_intervencao + " Descrição : " + i.descricao);
                }
            }
            
        }
    }


    static void Main(string[] args)
    {
        
        string input;
        while (true)
        {
            printOptionsMenu();
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    GetTeamWithQualifications();
                    break;
                case "2":
                    CreateIntervention();
                    break;
                case "3":
                    CreateTeam();
                    break;
                case "4":
                    AddElementToTeam();
                    break;
                case "5":
                    RemoveElementInTeam();
                    break;
                case "6":
                    printIntervencoesInYear();
                    break;
                case "7":
                    AddCompetencias();
                    break;
                case "8":
                    CreateInterventionNoProcedure();
                    break;
                case "9":
                    AttributeAnIntervention();
                    break;
                default:
                    Console.WriteLine("Escolha uma opção válida");
                    break;
            }
        }
    }

    private static void printOptionsMenu()
    {
        Console.WriteLine("1 : Obter equipa com as qualificações para fazer a intervenção");
        Console.WriteLine("2 : Criar uma intervenção");
        Console.WriteLine("3 : Criar uma equipa");
        Console.WriteLine("4 : Adicionar membro a uma equipa");
        Console.WriteLine("5 : Remover membro de uma equipa");
        Console.WriteLine("6 : Imprimir as intervenções de uma dado ano");
        Console.WriteLine("7 : Adicionar Competências a um funcionário");
        Console.WriteLine("8 : Criar intervenção sem procedimento");
        Console.WriteLine("9 : Associar uma intervenção a uma equipa");
    }
}