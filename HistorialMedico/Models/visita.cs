namespace HistorialMedico.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("visita")]
    public partial class visita
    {
        [Key]
        public int id_visita { get; set; }

        public int? id_paciente { get; set; }

        public int? id_doctor { get; set; }

        public byte[] fecha { get; set; }

        [StringLength(250)]
        public string motivo { get; set; }

        [StringLength(250)]
        public string comentario { get; set; }

        [StringLength(50)]
        public string recetaDeMedicmentos { get; set; }

        public DateTime? fechaProximaVisita { get; set; }

        public virtual doctor doctor { get; set; }

        public virtual paciente paciente { get; set; }

        [NotMapped]
        public List<paciente> ListaDePacientes { get; set; }
    }
}
