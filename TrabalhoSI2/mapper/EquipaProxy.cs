using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.dal;
using TrabalhoSI2.model;

namespace TrabalhoSI2.mapper
{
    class EquipaProxy : Equipa
    {
        private IContext ctx;

        public EquipaProxy(Equipa equipa, IContext ctx) : base()
        {
            this.ctx = ctx;

            base.codigo_equipa = equipa.codigo_equipa;
            base.id_supervisor = equipa.id_supervisor;
            base.localizacao = equipa.localizacao;
            base.num_elems = equipa.num_elems;
            base.TeamMembers = null;
            base.Intervencoes = null;
        }

 
        public override List<Funcionario> TeamMembers
        {
            get
            {
                if(base.TeamMembers == null)    //lazy load
                {
                    EquipaMapper equipaMapper = new EquipaMapper(ctx);
                    base.TeamMembers = equipaMapper.LoadTeamMembers(this);
                }
                return base.TeamMembers;
            }
            set
            {
                base.TeamMembers = value;
            }
        }

        public override List<Intervencao> Intervencoes
        {
            get
            {
                if(base.Intervencoes == null)
                {
                    EquipaMapper equipaMapper = new EquipaMapper(ctx);
                    base.Intervencoes = equipaMapper.LoadInterventions(this);
                }
                return base.Intervencoes;
            }
            set
            {
                base.Intervencoes = value;
            }

        }
    }
}
