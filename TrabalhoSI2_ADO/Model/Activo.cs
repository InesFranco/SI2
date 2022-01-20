using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public class Activo : IActivo
    {
        public int? activo_id { get ; set ; }
        public string nome { get ; set ; }
        public string data_aquisicao { get ; set ; }
        public bool estado { get ; set ; }
        public string marca { get ; set ; }
        public string modelo { get; set ; }
        public int tipo { get ; set ; }
    }
}
