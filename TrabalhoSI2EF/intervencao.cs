//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrabalhoSI2EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class intervencao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public intervencao()
        {
            this.intervencao_equipa = new HashSet<intervencao_equipa>();
        }
    
        public int id_intervencao { get; set; }
        public Nullable<int> id_activo { get; set; }
        public string descricao { get; set; }
        public string estado { get; set; }
        public double valor { get; set; }
        public System.DateTime data_inicio { get; set; }
        public System.DateTime data_fim { get; set; }
    
        public virtual activo activo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<intervencao_equipa> intervencao_equipa { get; set; }
    }
}