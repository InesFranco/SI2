using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.dal;
using TrabalhoSI2.helper;
using TrabalhoSI2.model;

namespace TrabalhoSI2.mapper
{
    public class EquipaMapper : IEquipaMapper
    {
        IContext _ctx;
        public EquipaMapper(IContext ctx)
        {
            _ctx = ctx;
        }
        public IEquipa Create(IEquipa entity)
        {
            entity.codigo_equipa = SQLMapperHelper.ExecuteScalar<int?>(_ctx, CommandType.StoredProcedure,
                "p_criaEquipa", 
                new IDbDataParameter[]{
                    new SqlParameter("@localizacao", entity.localizacao),
                    new SqlParameter("@id_supervisor", entity.id_supervisor)
                });
            return entity;
        }

        public IEquipa Delete(IEquipa entity)
        {
            throw new NotImplementedException();
        }

        public IEquipa EquipaMap(IDataRecord data)
        {
            IEquipa equipa = new Equipa();
            equipa.codigo_equipa = int.Parse(data[0].ToString());
            equipa.localizacao = data[1].ToString();
            equipa.num_elems = int.Parse(data[2].ToString());
            // TODO: Make the proxy to get the supervisor object
            equipa.id_supervisor = 1;
            return equipa;
            
        }



        //TODO: What happens when id is null?
        public IEquipa Read(int? id)
        {
            IEquipa equipa = (Equipa)SQLMapperHelper.ExecuteMapSingle(_ctx, "select * from equipa where codigo_equipa=@codigo_equipa", new IDbDataParameter[] { new SqlParameter("@codigo_equipa", id) }, EquipaMap);
            if (equipa != null)
                equipa.codigo_equipa = id;
            return equipa;
        }


        //TODO: add top so we only return top 10 for example
        public List<IEquipa> ReadAll(int top)
        {
            return SQLMapperHelper.ExecuteMapSet<IEquipa, List<IEquipa>>(_ctx, "select * from equipa", new IDbDataParameter[] {}, EquipaMap);
        }


        public IEquipa Update(IEquipa entity)
        {
            throw new NotImplementedException();
        }

    }
}
