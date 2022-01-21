using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2_ADO;
using TrabalhoSI2EF;

namespace TrabalhoSI2_ADO
{
    class MainApp
    {
        static void Main(string[] args)
        {
            bool ADO = true;
            Console.WriteLine("1: ADO App");
            Console.WriteLine("2: EF App");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ADO = true;
                    break;
                case "2":
                    ADO = false;
                    break;
                default:
                    Console.WriteLine("Escolha uma das duas opções");
                    break;
            }

            dynamic app;
            if (ADO) app = new ADOApp();
            else app = new EFApp();
           
            while (true)
            {
                printOptionsMenu();
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        app.GetTeamWithQualifications();
                        break;
                    case "2":
                        app.CreateIntervention();
                        break;
                    case "3":
                        app.CreateTeam();
                        break;
                    case "4":
                        app.AddElementToTeam();
                        break;
                    case "5":
                        app.RemoveElementInTeam();
                        break;
                    case "6":
                        app.printIntervencoesInYear();
                        break;
                    case "7":
                        app.AddCompetencias();
                        break;
                    case "8":
                        app.CreateInterventionNoProcedure();
                        break;
                    case "9":
                        app.AttributeAnIntervention();
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
}
