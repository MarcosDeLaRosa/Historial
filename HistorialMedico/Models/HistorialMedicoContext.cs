namespace HistorialMedico.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HistorialMedicoContext : DbContext
    {
        public HistorialMedicoContext()
            : base("name=HistorialMedicoContext")
        {
        }

        public virtual DbSet<asistente> asistente { get; set; }
        public virtual DbSet<cita> cita { get; set; }
        public virtual DbSet<doctor> doctor { get; set; }
        public virtual DbSet<paciente> paciente { get; set; }
        public virtual DbSet<pagoConsulta> pagoConsulta { get; set; }
        public virtual DbSet<precioConsulta> precioConsulta { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<visita> visita { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<asistente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<asistente>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<asistente>()
                .Property(e => e.telefono)
                .IsUnicode(false);

            modelBuilder.Entity<asistente>()
                .Property(e => e.correo)
                .IsUnicode(false);

            modelBuilder.Entity<cita>()
                .Property(e => e.duracion)
                .IsUnicode(false);

            modelBuilder.Entity<doctor>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<doctor>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<doctor>()
                .Property(e => e.telefono)
                .IsUnicode(false);

            modelBuilder.Entity<doctor>()
                .Property(e => e.correo)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.cedula)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.fechaNacimiento)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.correo)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.telefono)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .Property(e => e.tipodeSangre)
                .IsUnicode(false);

            modelBuilder.Entity<paciente>()
                .HasMany(e => e.cita)
                .WithOptional(e => e.paciente)
                .HasForeignKey(e => e.id_pasiente);

            modelBuilder.Entity<usuario>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.Correo)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.usuario1)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.roles)
                .IsUnicode(false);

            modelBuilder.Entity<visita>()
                .Property(e => e.motivo)
                .IsUnicode(false);

            modelBuilder.Entity<visita>()
                .Property(e => e.comentario)
                .IsUnicode(false);

            modelBuilder.Entity<visita>()
                .Property(e => e.recetaDeMedicmentos)
                .IsUnicode(false);
        }
    }
}
