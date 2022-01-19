using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public class Intervencao : IIntervencao
    {
        public int? id_intervencao { get ; set ; }
        public int? id_activo { get; set ; }
        public string descricao { get ; set ; }
        public string? estado { get; set ; }
        public float valor { get; set ; }
        public DateTime dataInicio { get; set ; }
        public DateTime dataFim { get ; set ; }
    }
}
