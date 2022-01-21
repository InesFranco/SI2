using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.concrete;
using TrabalhoSI2.dal;
using TrabalhoSI2.mapper;
using TrabalhoSI2.model;
using System.Globalization;
using System.Data.SqlClient;
using TrabalhoSI2.helper;
using System.Data;
using System;
using TrabalhoSI2EF;
using TrabalhoSI2_ADO.Model;


namespace TrabalhoSI2_ADO
{
    class ADOApp : IApplication<Intervencao, Equipa>
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ex1cs"].ConnectionString;

        public void AddCompetencias()
        {
            int idCompetencia;
            int idFuncionario;
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public void AddElementToTeam()
        {
            printTeams(10);

            Equipa equipa = new Equipa();
            int idFuncionario;
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
            using (IContext ctx = new Context(connectionString))
            {

                EquipaMapper equipaMapper = new EquipaMapper(ctx);
                try
                {
                    equipa = (Equipa)equipaMapper.UpdateAddTeamMembers(equipa, idFuncionario);
                    equipa = (Equipa)equipaMapper.Read(equipa.codigo_equipa);
                    printTeam(equipa);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public void AttributeAnIntervention()
        {

            int id_activo;
            string descricao;
            string estado;
            float valor;
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
                IEquipa equipa = (Equipa)GetTeamWithQualifications();
                if (equipa != null)
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

        public void CreateIntervention()
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
                    IntervencaoMapper intervencaoMapper = new IntervencaoMapper(ctx);

                    Intervencao intervencao = new Intervencao();
                    intervencao.id_activo = id_activo;
                    intervencao.descricao = descricao;
                    intervencao.valor = valor;
                    intervencao.dataInicio = dataInicio;
                    intervencao.dataFim = dataFim;

                    intervencao = intervencaoMapper.Create(intervencao);

                    if (intervencao.id_intervencao != null)
                    {
                        intervencao = intervencaoMapper.Read((int)intervencao.id_intervencao);
                        printIntervention(intervencao);
                    }
                }
            }
            catch (SqlException ex)
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

        public void CreateInterventionNoProcedure()
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

                if (dataAquisicao > dataInicio)
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
                    printIntervention(intervencao);
                }
            }
        }
        public void CreateTeam()
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
                }
                catch (SqlException ex)
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

        public Equipa GetTeamWithQualifications()
        {
            int intervencaoId;
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

            EquipaProxy equipa;
            using (IContext ctx = new Context(connectionString))
            {

                EquipaMapper equipaMapper = new EquipaMapper(ctx);
                equipa = (EquipaProxy)equipaMapper.ReadTeamWithQualifications(intervencaoId);
                if (equipa == null)
                {
                    Console.WriteLine("Não existe nenhuma equipa com essas capacidades/disponivel");
                    return null;
                }
            }

            Console.WriteLine("Equipa : " + equipa.codigo_equipa);
            Console.WriteLine("Localização : " + equipa.localizacao);
            Console.WriteLine("Número de Elementos : " + equipa.num_elems);
            Console.WriteLine("Supervisor : " + equipa.id_supervisor);
            foreach (Funcionario funcionario in equipa.TeamMembers)
            {
                Console.WriteLine("Funcionario: " + funcionario.nome + " profissão : " + funcionario.profissao);
            }
            Console.WriteLine();
            return equipa;
        }

        public void printIntervencoesInYear()
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

            if (intervenctions.Count() != 0)
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

        public void printIntervention(Intervencao intervencao)
        {
            Intervencao inter = (Intervencao)intervencao;
            Console.WriteLine();
            Console.WriteLine("id intervenção: " + inter.id_intervencao);
            Console.WriteLine("id activo: " + inter.id_activo);
            Console.WriteLine("descrição: " + inter.descricao);
            Console.WriteLine("estado: " + inter.estado);
            Console.WriteLine("valor : " + inter.valor);
            Console.WriteLine("data inicio: " + inter.dataInicio);
            Console.WriteLine("data fim : " + inter.dataFim);
            Console.WriteLine();
        }

        public void printTeam(Equipa equipa)
        {
            Equipa equipa1 = (Equipa)equipa;
            Console.WriteLine();
            Console.WriteLine("id Equipa: " + equipa1.codigo_equipa);
            Console.WriteLine("Localização: " + equipa1.localizacao);
            Console.WriteLine("Número de Elementos: " + equipa1.num_elems);
            Console.WriteLine("id Supervisor: " + equipa1.id_supervisor);

            Console.WriteLine("___________________________________________");
            Console.WriteLine("Elementos de equipa: ");
            foreach (Funcionario member in equipa1.TeamMembers)
            {
                Console.WriteLine("Membro: " + member.nome);
                Console.WriteLine("Email: " + member.email);
                Console.WriteLine("Endereço: " + member.endereco);
            }
            Console.WriteLine("___________________________________________");
            Console.WriteLine();
        }

        public void printTeams(int top)
        {
            using (IContext ctx = new Context(connectionString))
            {
                EquipaMapper mapper = new EquipaMapper(ctx);

                IList<IEquipa> equipas = mapper.ReadAll(top);

                foreach (Equipa e in equipas)
                {
                    printTeam(e);
                }
            }
        }

        public void RemoveElementInTeam()
        {

            int idFuncionario;
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

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Valor Inválido");
                }
            }

            try
            {
                using (IContext ctx = new Context(connectionString))
                {
                    printTeams(10);

                    IEquipaMapper equipaMapper = new EquipaMapper(ctx);
                    equipa = (Equipa)equipaMapper.Read(equipa.codigo_equipa);
                    equipa = (Equipa)equipaMapper.UpdateRemoveTeamMember(equipa, idFuncionario);
                    foreach (Funcionario funcionario in equipa.TeamMembers)
                    {
                        Console.WriteLine("Membro: " + funcionario.nome + " profissão : " + funcionario.profissao);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}



