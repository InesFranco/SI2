using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.model;

namespace TrabalhoSI2.mapper
{
    public interface IEquipaMapper : IMapper<IEquipa, int?>
    {
        IEquipa UpdateRemoveTeamMember(IEquipa equipa, int funcionario);
        IEquipa UpdateAddTeamMembers(IEquipa equipa, int funcionario);

        IEquipa ReadTeamWithQualifications(int Idintervencao);

        IEquipa AssignIntervention(IEquipa equipa, Intervencao intervencao);
        
    }
}
