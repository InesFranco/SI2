using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using TrabalhoSI2_EF;

namespace TrabalhoSI2EF
{
    public class EFApp : IApplication<intervencao, equipa>
    {

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

            var context = new Project1Entities();
            var equipa = new equipa()
            {
                num_elems = 1,
                localizacao = localizacao,
                id_supervisor = idSupervisor,
            };
            context.equipas.Add(equipa);

            //TODO PRINT PRETTY
            context.equipas.ToList().ForEach(e => Console.WriteLine(e.codigo_equipa));
            context.SaveChanges();
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

            var context = new Project1Entities();
            var intervencao = context.p_criaIntervencao(id_activo, descricao, valor, dataInicio, dataFim);
            foreach (Nullable<decimal> result in intervencao)
            {
                intervencao i = context.intervencaos.Find(result);
                Console.WriteLine("id intervencao: " + i.id_intervencao);
                Console.WriteLine("id activo     : " + i.id_activo);
                Console.WriteLine("Descrição     : " + i.descricao);
                Console.WriteLine("Estado        : " + i.estado);
                Console.WriteLine("Valor         : " + i.valor);
                Console.WriteLine("data_inicio   : " + i.data_inicio);
                Console.WriteLine("data_fim      : " + i.data_fim);
            }
        }

        public equipa GetTeamWithQualifications()
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

            
            var context = new Project1Entities();
            var team = context.p_encontrarEquipaParaIntervencaoEF(intervencaoId);
            foreach (Nullable<decimal> result in team)
            {
                equipa e = context.equipas.Find(result);
                printTeam(e);
                return e;
            }
            return null;
        }

        public void AddElementToTeam()
        {
            var context = new Project1Entities();


            int idFuncionario;
            int codigoEquipa;

            while (true)
            {
                try
                {
                    Console.WriteLine("Acrescentar Elementos a que equipa?");
                    codigoEquipa = int.Parse(Console.ReadLine());

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
                context.p_adicionarElementoEquipa(codigoEquipa, idFuncionario);
                var team = context.equipas.Include(f => f.funcionarios).Where(e => e.codigo_equipa == codigoEquipa).FirstOrDefault();

                Console.WriteLine("Membros Equipa:");
                foreach (funcionario f in team.funcionarios)
                {
                    Console.WriteLine("id funcionario         : " + f.id_funcionario);
                    Console.WriteLine("número de identificação: " + f.numero_identificacao);
                    Console.WriteLine("nome                   : " + f.nome);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public void RemoveElementInTeam()
        {
            int idFuncionario;
            int codigoEquipa;
            while (true)
            {
                try
                {
                    Console.WriteLine("Retirar Elemento de que equipa?");
                    codigoEquipa = int.Parse(Console.ReadLine());

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
                var context = new Project1Entities();
                context.p_removerElementoEquipa(codigoEquipa, idFuncionario);
                var team = context.equipas.Include(e => e.funcionarios).Where(e => e.codigo_equipa == codigoEquipa).FirstOrDefault();

                Console.WriteLine("Membros Equipa:");
                foreach (funcionario f in team.funcionarios)
                {
                    Console.WriteLine("id funcionario         : " + f.id_funcionario);
                    Console.WriteLine("número de identificação: " + f.numero_identificacao);
                    Console.WriteLine("nome                   : " + f.nome);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

            var context = new Project1Entities();
            var interventions = context.f_listInterventionsOfYear(year).ToList();
            foreach (f_listInterventionsOfYear_Result i in interventions)
            {
                Console.WriteLine("id intervencao: " + i.id_intervencao);
                Console.WriteLine("Descrição     : " + i.descricao);
            }

        }

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


            using (var context = new Project1Entities())
            {
                context.p_adicionarCompetencias(idFuncionario, idCompetencia);
                var funcionario = context.funcionarios.ToList().Find(f => f.id_funcionario == idFuncionario);

                Console.WriteLine("id funcionario         : " + funcionario.id_funcionario);
                Console.WriteLine("número de identificação: " + funcionario.numero_identificacao);
                Console.WriteLine("nome                   : " + funcionario.nome);

                foreach (competencia c in funcionario.competencias)
                {
                    Console.WriteLine("id Competencia: " + c.id_competencia);
                    Console.WriteLine("descrição     : " + c.descricao);
                }
                context.SaveChanges();
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

            using (var ctx = new Project1Entities())
            {
                DateTime dataAquisicaoActivo = ctx.activoes.ToList().Find(a => a.activo_id == id_activo).data_aquisicao.Date;

                if (dataAquisicaoActivo > dataInicio)
                {
                    Console.WriteLine("Activo obtido depois da data de inicio da intervenção");
                    return;
                }

                var intervencao = new intervencao
                {
                    valor = valor,
                    id_activo = id_activo,
                    estado = "por atribuir",
                    descricao = descricao,
                    data_inicio = dataInicio,
                    data_fim = dataFim
                };

                ctx.intervencaos.Add(intervencao);
                ctx.SaveChanges();

                Console.WriteLine("id intervencao: " + intervencao.id_intervencao);
                Console.WriteLine("Descrição     : " + intervencao.descricao);
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


            var intervencao = new intervencao
            {
                id_activo = id_activo,
                descricao = descricao,
                valor = valor,
                estado = estado,
                data_inicio = dataInicio,
                data_fim = dataFim
            };

            using (var ctx = new Project1Entities())
            {

                ctx.intervencaos.Add(intervencao);

                Console.WriteLine("id intervenção : " + intervencao.id_intervencao + " descrição: " + intervencao.descricao);

                //Assign a team
                var equipaId = ctx.p_encontrarEquipaParaIntervencaoEF(intervencao.id_intervencao).First();
                if (equipaId != null)
                {
                    var equipa = ctx.equipas.ToList().Find(e => e.codigo_equipa == equipaId);
                    if (equipa != null)
                    {
                        var intervencao_equipa = new intervencao_equipa()
                        {
                            codigo_equipa = equipa.codigo_equipa,
                            id_intervencao = intervencao.id_intervencao,
                            data_inicio = intervencao.data_inicio
                        };
                        ctx.intervencao_equipa.Add(intervencao_equipa);
                    }
                    ctx.SaveChanges();
                }


            }
        }

        public void printTeams(int top)
        {
            throw new NotImplementedException();
        }

        public void printIntervention(intervencao intervencao)
        {
            throw new NotImplementedException();
        }

        public void printTeam(equipa equipa)
        {
            Console.WriteLine("código equipa        : " + equipa.codigo_equipa);
            Console.WriteLine("localização          : " + equipa.localizacao);
            Console.WriteLine("numero de elementos  : " + equipa.num_elems);
            Console.WriteLine("id supervisor        : " + equipa.id_supervisor);
        }

        public void printIntervention(Project1Entities intervencao)
        {
            throw new NotImplementedException();
        }
    }
}




