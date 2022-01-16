﻿using System;
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

        public Intervencao Delete(Intervencao entity)
        {
            throw new NotImplementedException();
        }

        public Intervencao Read(int id)
        {
            throw new NotImplementedException();
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
