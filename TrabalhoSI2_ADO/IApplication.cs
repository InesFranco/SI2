using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2_ADO
{
    public interface IApplication<I,T>
    {
        void printTeams(int top);
        void printIntervention(I intervencao);


        void printTeam(T equipa);

        T GetTeamWithQualifications();

        void CreateIntervention();

        void CreateTeam();


        void printIntervencoesInYear();


        void AddElementToTeam();

        void RemoveElementInTeam();

        void AddCompetencias();

        void CreateInterventionNoProcedure();

        void AttributeAnIntervention();
    }
}
