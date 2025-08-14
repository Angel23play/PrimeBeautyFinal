using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Models;

public partial class PrimebeautyContext : DbContext
{
    public PrimebeautyContext()
    {
    }

    public PrimebeautyContext(DbContextOptions<PrimebeautyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FacturasDetalle> FacturasDetalles { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=primebeauty;user=root;password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("citas");

            entity.HasIndex(e => e.EmpleadoId, "fk_Citas_Empleados1_idx");

            entity.HasIndex(e => e.UsuarioId, "fk_Citas_Usuarios1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpleadoId).HasColumnName("empleado_id");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('pendiente','aprobada','finalizada','cancelada')")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasMaxLength(45)
                .HasColumnName("fecha");
            entity.Property(e => e.Hora)
                .HasMaxLength(45)
                .HasColumnName("hora");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Cita)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Citas_Empleados1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Cita)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Citas_Usuarios1");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(45)
                .HasColumnName("apellido");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(45)
                .HasColumnName("telefono");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("facturas");

            entity.HasIndex(e => e.CitaId, "fk_Facturas_Citas1_idx");

            entity.HasIndex(e => e.UsuarioId, "fk_Facturas_Usuarios1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CitaId).HasColumnName("cita_id");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Cita).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.CitaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Facturas_Citas1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Facturas_Usuarios1");
        });

        modelBuilder.Entity<FacturasDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("facturas_detalles");

            entity.HasIndex(e => e.ServicioId, "fk_Facturas_detalles_Servicios1_idx");

            entity.HasIndex(e => e.FacturaId, "fk_Producto_detalles_Facturas1_idx");

            entity.HasIndex(e => e.ProductoId, "fk_Producto_detalles_Productos_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precio unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.ServicioId).HasColumnName("servicio_id");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.Factura).WithMany(p => p.FacturasDetalles)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Producto_detalles_Facturas1");

            entity.HasOne(d => d.Producto).WithMany(p => p.FacturasDetalles)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("fk_Producto_detalles_Productos");

            entity.HasOne(d => d.Servicio).WithMany(p => p.FacturasDetalles)
                .HasForeignKey(d => d.ServicioId)
                .HasConstraintName("fk_Facturas_detalles_Servicios1");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.FacturaId, "fk_Pagos_Facturas1_idx");

            entity.HasIndex(e => e.UsuarioId, "fk_Pagos_Usuarios1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cuenta)
                .HasMaxLength(225)
                .HasColumnName("cuenta");
            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.MetodoDePago)
                .HasColumnType("enum('transferencia','tarjeta','efectivo')")
                .HasColumnName("metodo_de_pago");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.Nota)
                .HasMaxLength(45)
                .HasColumnName("nota");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Factura).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pagos_Facturas1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pagos_Usuarios1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadExistente).HasColumnName("cantidad_existente");
            entity.Property(e => e.Categoria)
                .HasColumnType("enum('belleza','bebida','consumible')")
                .HasColumnName("categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(245)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("servicios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(45)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(45)
                .HasColumnName("telefono");
            entity.Property(e => e.Tipo)
                .HasColumnType("enum('admin','cliente','recepcionista')")
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
