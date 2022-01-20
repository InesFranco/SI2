using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public interface IEquipa
    {
        int? codigo_equipa { get; set; }
        string localizacao { get; set; }
        int num_elems { get; set; }
        int id_supervisor { get; set; }

        List<Funcionario> TeamMembers { get; set; }
        List<Intervencao> Intervencoes { get; set; }

    }
}
