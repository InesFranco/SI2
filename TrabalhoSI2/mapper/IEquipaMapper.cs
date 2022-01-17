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
        public IEquipa UpdateRemoveTeamMember(IEquipa equipa, int funcionario);
        public IEquipa UpdateAddTeamMembers(IEquipa equipa, int funcionario);
        
    }
}
