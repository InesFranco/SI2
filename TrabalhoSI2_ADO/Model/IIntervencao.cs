using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2_ADO.Model;

namespace TrabalhoSI2.model
{
    public interface IIntervencao : IEntity
    {
        int? id_intervencao { get; set; }
        int? id_activo { get; set; }
        string descricao { get; set; }
        string estado   { get; set; }
        float valor { get; set; }
        DateTime dataInicio { get; set; }
        DateTime dataFim { get; set; }
    }
}
