using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class MyContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<CajaDeAhorro> Cajas { get; set; }
        public DbSet<CajaAhorroUsuario> CajasUsuario { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PlazoFijo> Plazos { get; set; }

        public MyContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Resources.ConnectionStr);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario")
                .HasKey(u => u.IdUsuario);
            //propiedades de los datos
            modelBuilder.Entity<Usuario>(
                usr =>
                {
                    usr.Property(u => u.Dni).HasColumnType("int");
                    usr.Property(u => u.Dni).IsRequired(true);
                    usr.Property(u => u.Nombre).HasColumnType("varchar(50)");
                    usr.Property(u => u.Nombre).IsRequired(true);
                    usr.Property(u => u.Apellido).HasColumnType("varchar(50)");
                    usr.Property(u => u.Apellido).IsRequired(true);
                    usr.Property(u => u.Mail).HasColumnType("varchar(50)");
                    usr.Property(u => u.Mail).IsRequired(true);
                    usr.Property(u => u.Clave).HasColumnType("varchar(50)");
                    usr.Property(u => u.Clave).IsRequired(true);
                    usr.Property(u => u.IsAdmin).HasColumnType("bit");
                    usr.Property(u => u.IsAdmin).IsRequired(true);
                    usr.Property(u => u.IsBloqueado).HasColumnType("bit");
                    usr.Property(u => u.IsBloqueado).IsRequired(true);
                });

            modelBuilder.Entity<Tarjeta>()
                .ToTable("Tarjeta")
                .HasKey(t => t.IdTarjeta);
            modelBuilder.Entity<Tarjeta>(
                tar =>
                {
                    tar.Property(t => t.Numero).HasColumnType("int");
                    tar.Property(t => t.Numero).IsRequired(true);
                    tar.Property(t => t.Consumos).HasColumnType("real");
                    tar.Property(t => t.Consumos).IsRequired(true);
                    tar.Property(t => t.CodigoV).HasColumnType("int");
                    tar.Property(t => t.CodigoV).IsRequired(true);
                    tar.Property(t => t.Limite).HasColumnType("real");
                    tar.Property(t => t.Limite).IsRequired(true);
                    tar.Property(t => t.IdUsuario).HasColumnType("int");
                    tar.Property(t => t.IdUsuario).IsRequired(true);
                });

            modelBuilder.Entity<Movimiento>()
                .ToTable("Movimiento")
                .HasKey(m => m.IdMovimiento);
            modelBuilder.Entity<Movimiento>(
                mov =>
                {
                    mov.Property(m => m.Detalle).HasColumnType("varchar(50)");
                    mov.Property(m => m.Detalle).IsRequired(true);
                    mov.Property(m => m.Monto).HasColumnType("real");
                    mov.Property(m => m.Monto).IsRequired(true);
                    mov.Property(m => m.Fecha).HasColumnType("date");
                    mov.Property(m => m.Fecha).IsRequired(true);
                    mov.Property(m => m.IdCajaAhorro).HasColumnType("int");
                    mov.Property(m => m.IdCajaAhorro).IsRequired(true);
                });

            modelBuilder.Entity<Pago>()
                .ToTable("Pago")
                .HasKey(p => p.IdPago);
            modelBuilder.Entity<Pago>(
                pag =>
                {
                    pag.Property(p => p.Detalle).HasColumnType("varchar(50)");
                    pag.Property(p => p.Detalle).IsRequired(true);
                    pag.Property(p => p.Monto).HasColumnType("real");
                    pag.Property(p => p.Monto).IsRequired(true);
                    pag.Property(p => p.IsPagado).HasColumnType("bit");
                    pag.Property(p => p.IsPagado).IsRequired(true);
                    pag.Property(p => p.Metodo).HasColumnType("varchar(50)");
                    pag.Property(p => p.Metodo).IsRequired(true);
                    pag.Property(p => p.IdUsuario).HasColumnType("int");
                    pag.Property(p => p.IdUsuario).IsRequired(true);
                });

            modelBuilder.Entity<PlazoFijo>()
                .ToTable("PlazoFijo")
                .HasKey(p => p.IdPlazoFijo);
            modelBuilder.Entity<PlazoFijo>(
                pla =>
                {
                    pla.Property(p => p.Monto).HasColumnType("real");
                    pla.Property(p => p.Monto).IsRequired(true);
                    pla.Property(p => p.Tasa).HasColumnType("real");
                    pla.Property(p => p.Tasa).IsRequired(true);
                    pla.Property(p => p.FechaIni).HasColumnType("date");
                    pla.Property(p => p.FechaIni).IsRequired(true);
                    pla.Property(p => p.FechaFin).HasColumnType("date");
                    pla.Property(p => p.FechaFin).IsRequired(true);
                    pla.Property(p => p.IsPagado).HasColumnType("bit");
                    pla.Property(p => p.IsPagado).IsRequired(true);
                    pla.Property(p => p.IdUsuario).HasColumnType("int");
                    pla.Property(p => p.IdUsuario).IsRequired(true);
                    pla.Property(p => p.CbuAPagar).HasColumnType("int");
                    pla.Property(p => p.CbuAPagar).IsRequired(true);
                });

            modelBuilder.Entity<CajaDeAhorro>()
                .ToTable("CajaDeAhorro")
                .HasKey(c => c.IdCajaAhorro);
            modelBuilder.Entity<CajaDeAhorro>(
                caja =>
                {
                    caja.Property(c => c.Cbu).HasColumnType("int");
                    caja.Property(c => c.Cbu).IsRequired(true);
                    caja.Property(c => c.Saldo).HasColumnType("real");
                });


            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY CAJA DE AHORRO -> MOVIMIENTOS
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.CajaDeAhorro)
                .WithMany(ca => ca.Movimientos)
                .HasForeignKey(m => m.IdCajaAhorro)
                .OnDelete(DeleteBehavior.Cascade);

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> PAGOS
            modelBuilder.Entity<Pago>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Pagos)
            .HasForeignKey(p => p.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> PLAZO FIJO
            modelBuilder.Entity<PlazoFijo>()
                .HasOne(pf => pf.Usuario)
                .WithMany(u => u.PlazosFijos)
                .HasForeignKey(pf => pf.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> TARJETA DE CREDITO
            modelBuilder.Entity<Tarjeta>()
            .HasOne(tr => tr.usuario)
            .WithMany(u => u.Tarjetas)
            .HasForeignKey(tr => tr.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO <-> CAJA DE AHORRO
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Cajas)
                .WithMany(ca => ca.Titulares)
                .UsingEntity<CajaAhorroUsuario>(
                euca => euca.HasOne(cau => cau.CajaDeAhorro).WithMany(ca => ca.CajaAhorroUsuarios).HasForeignKey(c => c.IdCajaAhorro),
                euca => euca.HasOne(cau => cau.Usuario).WithMany(u => u.CajaAhorroUsuarios).HasForeignKey(u => u.IdUsuario),
                euca => euca.HasKey(k => new { k.IdUsuario, k.IdCajaAhorro })
                );


            modelBuilder.Entity<Usuario>().HasData(
                new { IdUsuario = 1, Dni = 2, Nombre = "admin", Apellido= "admin",Mail = "admin@admin.com", Clave = "123", IntentosFallidos = 0, IsAdmin = true, IsBloqueado = false }
                ); 

            //Ignoro, no agrego UsuarioManager a la base de datos
            modelBuilder.Ignore<Banco>();
        }
    }
}
