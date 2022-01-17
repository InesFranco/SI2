using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.dal;
using TrabalhoSI2.helper;
using TrabalhoSI2.mapper;
using TrabalhoSI2.model;

namespace TrabalhoSI2.concrete
{
    public class FuncionarioMapper : IFuncionarioMapper
    {
        IContext _ctx = null;
        public FuncionarioMapper(IContext ctx)
        {
            _ctx = ctx;
        }
        public Funcionario Create(Funcionario entity)
        {
            throw new NotImplementedException();
        }

        public Funcionario Delete(Funcionario entity)
        {
            throw new NotImplementedException();
        }

        public Funcionario FuncionarioMap(IDataRecord dataRecord)
        {
            Funcionario funcionario = new Funcionario();
            funcionario.id_funcionario = int.Parse(dataRecord[0].ToString());
            funcionario.numero_identtificacao = int.Parse(dataRecord[1].ToString());
            funcionario.nome = dataRecord[2].ToString();
            funcionario.data_nascimento = DateTime.Parse(dataRecord[3].ToString());
            funcionario.endereco = dataRecord[4].ToString();
            funcionario.profissao = dataRecord[5].ToString();
            funcionario.telefone = int.Parse(dataRecord[6].ToString());
            funcionario.email = dataRecord[7].ToString();
            return funcionario;
        }

        public Funcionario Read(int id)
        {
            Funcionario funcionario = SQLMapperHelper.ExecuteMapSingle<Funcionario>(_ctx,"select * from funcionario where id_funcionario=@id",
                new IDbDataParameter[] {
                        new SqlParameter("@id", id)
                }, FuncionarioMap);
            return funcionario;
        }

        public List<Funcionario> ReadAll(int top)
        {
            throw new NotImplementedException();
        }

        public Funcionario Update(Funcionario entity)
        {
            throw new NotImplementedException();
        }
    }
}
