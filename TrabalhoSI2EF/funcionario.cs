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
    
    public partial class funcionario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public funcionario()
        {
            this.activoes = new HashSet<activo>();
            this.competencias = new HashSet<competencia>();
            this.equipas = new HashSet<equipa>();
        }
    
        public int id_funcionario { get; set; }
        public int numero_identificacao { get; set; }
        public string nome { get; set; }
        public Nullable<System.DateTime> data_nascimento { get; set; }
        public string endereco { get; set; }
        public string profissao { get; set; }
        public Nullable<int> telefone { get; set; }
        public string email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<activo> activoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<competencia> competencias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<equipa> equipas { get; set; }
    }
}
