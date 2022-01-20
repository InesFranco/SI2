using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public class Funcionario : IFuncionario
    {
        public Funcionario()
        {
            competencias = new List<Competencia>();
        }

        public int? id_funcionario { get ; set ; }
        public int numero_identtificacao { get ; set ; }
        public string nome { get; set ; }
        public DateTime data_nascimento { get; set; }
        public string endereco { get ; set ; }
        public string profissao { get ; set ; }
        public int telefone { get ; set ; }
        public string email { get ; set ; }
        public List<Competencia> competencias { get ; set ; }
    }
}
