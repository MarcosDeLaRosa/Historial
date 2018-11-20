namespace HistorialMedico.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cita")]
    public partial class cita
    {
        [Key]
        public int id_cita { get; set; }

        public int? id_pasiente { get; set; }

        public int? id_doctor { get; set; }

        public DateTime? fechaDeConsulta { get; set; }

        public DateTime? fechaDeCita { get; set; }

        public DateTime? hora { get; set; }

        [StringLength(50)]
        public string duracion { get; set; }

        public virtual doctor doctor { get; set; }

        public virtual paciente paciente { get; set; }
    }
}
