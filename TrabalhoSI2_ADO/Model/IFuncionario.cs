using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.model
{
    public interface IFuncionario
    {
        int? id_funcionario { get; set; }
        int numero_identtificacao { get; set; }
        string nome { get; set; }
        DateTime data_nascimento { get; set; }

        string endereco { get; set; }
        string profissao { get; set; }

        int telefone { get; set; }
        string email { get; set; }

        List<Competencia> competencias { get; set; }

    }
}
