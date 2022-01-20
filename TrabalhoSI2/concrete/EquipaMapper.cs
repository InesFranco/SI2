using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.concrete;
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
            try
            {
                entity.codigo_equipa = (int)SQLMapperHelper.ExecuteScalar<decimal>(_ctx, CommandType.StoredProcedure,
                "p_criaEquipa",
                new IDbDataParameter[]{
                    new SqlParameter("@localizacao", entity.localizacao),
                    new SqlParameter("@id_supervisor", entity.id_supervisor)
                });

            }catch (Exception ex)
            {
                throw;
            }
            return new EquipaProxy((Equipa)entity, _ctx);
        }

        public List<Funcionario> LoadTeamMembers(IEquipa equipa)
        {
            //Add supervisor as team member
            FuncionarioMapper funcionarioMapper = new FuncionarioMapper(_ctx);

            SqlCommand cmd = _ctx.createCommand();
            cmd.CommandText = "select id_funcionario from funcionario_equipa where @codigo_equipa=codigo_equipa";
            cmd.Parameters.Add(new SqlParameter("@codigo_equipa", equipa.codigo_equipa));

            List<Funcionario> teamMembers = new List<Funcionario>();

            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    teamMembers.Add(funcionarioMapper.Read(reader.GetInt32(0)));
                }
            }
                
            return teamMembers;
        }


        public List<Intervencao> LoadInterventions(IEquipa equipa)
        {
            //Add all the interventions assigned to this team
            SqlCommand cmd = _ctx.createCommand();
            cmd.CommandText = "select id_intervencao from intervencao_equipa where @codigo_equipa=codigo_equipa";
            cmd.Parameters.Add(new SqlParameter("@codigo_equipa", equipa.codigo_equipa));

            List<Intervencao> intervencoes = new List<Intervencao>();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                IntervencaoMapper intervencaoMapper = new IntervencaoMapper(_ctx);
                while (dr.Read())
                {
                    intervencoes.Add(intervencaoMapper.Read(dr.GetInt32(0)));
                }
            }
            return intervencoes;

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
            if(id != null)
            {
                IEquipa equipa = (Equipa)SQLMapperHelper.ExecuteMapSingle(_ctx, "select * from equipa where codigo_equipa=@codigo_equipa",
                new IDbDataParameter[] {
                    new SqlParameter("@codigo_equipa", id)
                }, EquipaMap);

                return new EquipaProxy((Equipa)equipa, _ctx);
            }
            return null;
        }


        //TODO: add top so we only return top 10 for example
        public List<IEquipa> ReadAll(int top)
        {
            return SQLMapperHelper.ExecuteMapSet<IEquipa, List<IEquipa>>(_ctx, "select * from equipa", new IDbDataParameter[] {}, EquipaMap);
        }

        public IEquipa UpdateAddTeamMembers(IEquipa entity, int idFuncionario)
        {
            try
            {
                SQLMapperHelper.ExecuteNonQuery(_ctx, CommandType.StoredProcedure, "p_adicionarElementoEquipa",
                new IDbDataParameter[]
                {
                    new SqlParameter("@id_equipa", entity.codigo_equipa),
                    new SqlParameter("@id_funcionario", idFuncionario)

                });
                return entity;
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

        public IEquipa UpdateRemoveTeamMember(IEquipa entity, int idFuncionario)
        {
            try
            {
                SQLMapperHelper.ExecuteNonQuery(_ctx, CommandType.StoredProcedure, "p_removerElementoEquipa",
                new IDbDataParameter[]
                {
                    new SqlParameter("@id_equipa", entity.codigo_equipa),
                    new SqlParameter("@id_funcionario", idFuncionario)

                });

                return entity;
            }catch (Exception ex)
            {
                throw;
            }
            
        }



        //
        public IEquipa Update(IEquipa entity)
        {
            throw new NotImplementedException();
        }

        public IEquipa ReadTeamWithQualifications(int Idintervencao)
        {
            int? idEquipa = SQLMapperHelper.ExecuteScalar<int?>(_ctx, 
                CommandType.Text, 
                "select dbo.encontrarEquipaParaIntervencao(@id_intervencao)", 
                new IDbDataParameter[] 
                {
                    new SqlParameter("@id_intervencao", Idintervencao) 
                });
            return new EquipaProxy((Equipa)Read(Idintervencao), _ctx);
        }

        public IEquipa AssignIntervention(IEquipa equipa, Intervencao intervencao)
        {
            SQLMapperHelper.ExecuteNonQuery(_ctx, CommandType.Text, "insert into intervencao_equipa(codigo_equipa, id_intervencao, data_inicio) values(@codigo_equipa, @id_intervencao, @data_inicio)",
                new IDbDataParameter[]
                {
                    new SqlParameter("@codigo_equipa", equipa.codigo_equipa),
                    new SqlParameter("@id_intervencao", intervencao.id_intervencao),
                    new SqlParameter("@data_inicio", intervencao.dataInicio)
                });
            return new EquipaProxy((Equipa)Read(equipa.codigo_equipa), _ctx);
        }
    }
}
