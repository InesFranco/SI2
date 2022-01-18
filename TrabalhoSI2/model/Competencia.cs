using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public class Competencia : ICompetencia
    {
        public int? idCompetencia { get ; set ; }
        public string descricao { get ; set ; }
    }
}
