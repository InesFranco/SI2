using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IApplication<T>
    {
        void printTeams(int top);
        void printIntervention(T intervencao);


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
