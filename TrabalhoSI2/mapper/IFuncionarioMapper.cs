using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.model;

namespace TrabalhoSI2.mapper
{
    public interface IFuncionarioMapper : IMapper<Funcionario,int>
    {
        public Funcionario AddCompetencia(Funcionario entity, int idCompetencia);
    }
}
