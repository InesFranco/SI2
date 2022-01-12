using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public interface IActivo
    {
        int? activo_id { get; set; }
        string nome { get; set; }
        string data_aquisicao { get; set; }
        bool estado { get; set; }
        string? marca { get; set; }
        string? modelo { get; set; }
        int tipo { get; set; }

    }
}
