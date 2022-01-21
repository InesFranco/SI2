using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class Program
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

            ADOApp adoApp = new ADOApp();

            while (true)
            {
                printOptionsMenu();
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        if (ADO) ADOApp.GetTeamWithQualifications();
                        else EFApp.GetTeamWithQualifications();
                        break;
                    case "2":
                        if (ADO) ADOApp.CreateIntervention();
                        else EFApp.CreateIntervention();
                        break;
                    case "3":
                        if (ADO) ADOApp.CreateTeam();
                        else EFApp.CreateTeam();
                        break;
                    case "4":
                        if (ADO) ADOApp.AddElementToTeam();
                        else EFApp.AddElementToTeam();
                        break;
                    case "5":
                        if (ADO) ADOApp.RemoveElementInTeam();
                        else EFApp.RemoveElementInTeam();
                        break;
                    case "6":
                        if (ADO) ADOApp.printIntervencoesInYear();
                        else EFApp.printIntervencoesInYear();
                        break;
                    case "7":
                        if (ADO) ADOApp.AddCompetencias();
                        else EFApp.AddCompetencias();
                        break;
                    case "8":
                        if (ADO) ADOApp.CreateInterventionNoProcedure();
                        else EFApp.CreateInterventionNoProcedure();
                        break;
                    case "9":
                        if (ADO) ADOApp.AttributeAnIntervention();
                        else EFApp.AttributeAnIntervention();
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
