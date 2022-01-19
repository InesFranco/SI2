using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2EF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new Project1Entities();
            var intervencao = new intervencao()
            {
                id_activo = 1,
                descricao = "macaco",
                estado = "em execução",
                valor = 32,
                data_inicio = new DateTime(2022, 04, 22),
                data_fim = new DateTime(2022, 04, 25)
            };
            context.intervencaos.Add(intervencao);
            context.SaveChanges();
        }
    }
}
