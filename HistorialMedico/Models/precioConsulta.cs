namespace HistorialMedico.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("precioConsulta")]
    public partial class precioConsulta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public precioConsulta()
        {
            pagoConsulta = new HashSet<pagoConsulta>();
        }

        [Key]
        public int id_precioConsulta { get; set; }

        public int? id_doctor { get; set; }

        public int? costo { get; set; }

        public virtual doctor doctor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pagoConsulta> pagoConsulta { get; set; }
    }
}
