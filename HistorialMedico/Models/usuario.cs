namespace HistorialMedico.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("usuario")]
    public partial class usuario
    {
        [Key]
        public int id_usuario { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellido { get; set; }

        [StringLength(50)]
        public string Correo { get; set; }

        public byte[] Foto { get; set; }

        [Column("usuario")]
        [StringLength(50)]
        [DisplayName("Usuario")]
        public string usuario1 { get; set; }

        [StringLength(50)]
        [DisplayName("Contraseña")]
        public string contrasena { get; set; }

        [StringLength(50)]
        public string roles { get; set; }
    }
}
