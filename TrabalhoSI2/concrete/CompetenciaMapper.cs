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
    public class CompetenciaMapper : ICompetenciaMapper
    {
        public IContext _ctx;

        public CompetenciaMapper(IContext ctx)
        {
            _ctx = ctx;
        }
        public Competencia Create(Competencia entity)
        {
            throw new NotImplementedException();
        }

        public Competencia Delete(Competencia entity)
        {
            throw new NotImplementedException();
        }


        public Competencia CompetenciaMap(IDataRecord dataRecord)
        {
            Competencia competencia = new Competencia();
            competencia.idCompetencia = int.Parse(dataRecord[0].ToString());
            competencia.descricao = dataRecord[1].ToString();
            return competencia;
        }

        public Competencia Read(int id)
        {
            return SQLMapperHelper.ExecuteMapSingle(_ctx, "select * from competencia where id_competencia=@id_competencia",
                new IDbDataParameter[]
                {
                    new SqlParameter("@id_competencia", id)
                }, CompetenciaMap);
        }

        public List<Competencia> ReadAll(int top)
        {
            throw new NotImplementedException();
        }

        public Competencia Update(Competencia entity)
        {
            throw new NotImplementedException();
        }
    }
}
