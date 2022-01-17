using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public class Equipa : IEquipa
    {
        public Equipa()
        {
            TeamMembers = new List<Funcionario>();
        }
        public int? codigo_equipa { get ; set; }
        public string localizacao { get; set ; }
        public int num_elems { get ; set; }
        public int id_supervisor { get ; set ;}
        public List<Funcionario> TeamMembers { get; set; }
    }
}
