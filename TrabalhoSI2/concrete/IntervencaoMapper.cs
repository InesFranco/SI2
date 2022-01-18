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
    public class IntervencaoMapper : IIntervencaoMapper
    {
        public IContext _ctx;

        public IntervencaoMapper(IContext ctx)
        {
            _ctx = ctx;
        }
        public Intervencao Create(Intervencao entity)
        {
           try
            {
                entity.id_intervencao = SQLMapperHelper.ExecuteScalar<int?>(_ctx, CommandType.StoredProcedure, "p_criaIntervencao",
                new IDbDataParameter[] {
                    new SqlParameter("@id_activo", entity.id_activo),
                    new SqlParameter("@descricao", entity.descricao),
                    new SqlParameter("@valor", entity.valor),
                    new SqlParameter("@data_inicio", entity.dataInicio),
                    new SqlParameter("@data_fim", entity.dataFim)
                }); 
                
                return entity;
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            
        }

        public Intervencao CreateNoProcedure(Intervencao entity)
        {
            try
            {
                entity.id_intervencao =  SQLMapperHelper.ExecuteScalar<int?>(_ctx, CommandType.Text, "Insert into intervencao (id_activo, descricao, estado, valor, data_inicio, data_fim) values(@id_activo, @descricao, @estado, @valor, @data_inicio, @data_fim)",
                new IDbDataParameter[]
                {
                    new SqlParameter("@id_activo", entity.id_activo),
                    new SqlParameter("@descricao", entity.descricao),
                    new SqlParameter("@estado", entity.estado),
                    new SqlParameter("@valor",entity.valor),
                    new SqlParameter("@data_inicio", entity.dataInicio),
                    new SqlParameter("@data_fim", entity.dataFim)
                });
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public Intervencao Delete(Intervencao entity)
        {
            throw new NotImplementedException();
        }

        public Intervencao Read(int id)
        {
            return SQLMapperHelper.ExecuteMapSingle(_ctx, "select * from intervencao where @id_intervencao=id_intervencao",
                new IDbDataParameter[]
                {
                    new SqlParameter("@id_intervencao", id)
                }, intervencaoMap);
        }

        public Intervencao intervencaoMap(IDataRecord record)
        {
            Intervencao intervencao = new Intervencao();
            intervencao.id_intervencao = int.Parse(record[0].ToString());
            intervencao.descricao = record[1].ToString();
            return intervencao;
        }

        public List<Intervencao> ReadAllYear(int ano)
        {
            return SQLMapperHelper.ExecuteMapSet<Intervencao, List<Intervencao>>(_ctx, "select * from f_listInterventionsOfYear(@ano)",
                new IDbDataParameter[] { new SqlParameter("@ano", ano) }, intervencaoMap);
        }

        public List<Intervencao> ReadAll(int top)
        {
            throw new NotImplementedException();
        }

        public Intervencao Update(Intervencao entity)
        {
            throw new NotImplementedException();
        }
    }
}
