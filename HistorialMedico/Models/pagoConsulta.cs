namespace HistorialMedico.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pagoConsulta")]
    public partial class pagoConsulta
    {
        [Key]
        public int id_pagoConsulta { get; set; }

        public int? id_Paciente { get; set; }

        public int? id_PrecioConsulta { get; set; }

        public int? pago { get; set; }

        public int? total { get; set; }

        public DateTime? fecha { get; set; }

        public virtual paciente paciente { get; set; }

        public virtual precioConsulta precioConsulta { get; set; }
    }
}
