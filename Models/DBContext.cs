using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpediFlow.Models;

public partial class DBContext : IdentityDbContext<Usuario>
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<DiaFestivo> DiaFestivos { get; set; }

    public virtual DbSet<Dictaman> Dictamen { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<DivisionDepartamento> DivisionDepartamentos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Entidad> Entidads { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<ExpedienteArchivoAdjunto> ExpedienteArchivoAdjuntos { get; set; }

    public virtual DbSet<ExpedienteDetalle> ExpedienteDetalles { get; set; }

    public virtual DbSet<ExpedienteNotificacion> ExpedienteNotificacions { get; set; }

    public virtual DbSet<ExpedienteRequisito> ExpedienteRequisitos { get; set; }

    public virtual DbSet<Flujo> Flujos { get; set; }

    public virtual DbSet<FlujoBloque> FlujoBloques { get; set; }

    public virtual DbSet<FlujoTransicion> FlujoTransicions { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Requisito> Requisitos { get; set; }

    public virtual DbSet<Resolucion> Resolucions { get; set; }

    public virtual DbSet<RoleVentana> RoleVentanas { get; set; }

    public virtual DbSet<SubTramite> SubTramites { get; set; }

    public virtual DbSet<SubTramiteRequisito> SubTramiteRequisitos { get; set; }

    public virtual DbSet<TipoEntidad> TipoEntidads { get; set; }

    public virtual DbSet<TipoEstado> TipoEstados { get; set; }

    public virtual DbSet<Tramite> Tramites { get; set; }

    public virtual DbSet<Unidad> Unidads { get; set; }

    public virtual DbSet<Ventana> Ventanas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:sDBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433D0DF385E3");

            entity.ToTable("Departamento");

            entity.HasIndex(e => e.Nombre, "IX_Departamento").IsUnique();

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DiaFestivo>(entity =>
        {
            entity.HasKey(e => e.IdDiaFestivo);

            entity.ToTable("DiaFestivo");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(8)
                .HasDefaultValue("#4ADEDE");
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaHasta).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreDiaFestivo)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Dictaman>(entity =>
        {
            entity.HasKey(e => e.IdDictamen).HasName("PK_Dictament");

            entity.HasIndex(e => e.NumeroDictamen, "IX_Dictamen").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioDictamen)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Justificacion).IsRequired();
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NumeroDictamen)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.Dictamen)
                .HasForeignKey(d => d.IdExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dictamen_Expediente");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.IdDivision);

            entity.ToTable("Division");

            entity.HasIndex(e => e.NombreDivision, "IX_Division").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NombreDivision)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<DivisionDepartamento>(entity =>
        {
            entity.HasKey(e => e.IdDivisionDepartamento).HasName("PK_Departamento");

            entity.ToTable("DivisionDepartamento");

            entity.HasIndex(e => e.NombreDepartamento, "IX_DivisionDepartamento").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreDepartamento)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.DivisionDepartamentos)
                .HasForeignKey(d => d.IdDivision)
                .HasConstraintName("FK_DivisionDepartamento_Division");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa);

            entity.ToTable("Empresa");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Comentarios).HasMaxLength(4000);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreContacto).HasMaxLength(100);
            entity.Property(e => e.NombreEmpresa)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Rtn)
                .HasMaxLength(20)
                .HasColumnName("RTN");
            entity.Property(e => e.Siglas).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(10);
            entity.Property(e => e.TelefonoContacto).HasMaxLength(10);
        });

        modelBuilder.Entity<Entidad>(entity =>
        {
            entity.HasKey(e => e.IdEntidad).HasName("PK__Cliente__D5946642003FB668");

            entity.ToTable("Entidad");

            entity.HasIndex(e => new { e.Nombre, e.Dni, e.IdTipoEntidad }, "IX_Entidad").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo).HasMaxLength(10);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DNI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdTipoEntidad)
                .HasDefaultValue(1)
                .HasComment("1: Persona Natural, 2: Institución, 3: Apoderado");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rtn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("RTN");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Entidads)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Departamento");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Entidads)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Municipio");

            entity.HasOne(d => d.IdTipoEntidadNavigation).WithMany(p => p.Entidads)
                .HasForeignKey(d => d.IdTipoEntidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entidad_TipoEntidad");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK_Estdado");

            entity.ToTable("Estado");

            entity.HasIndex(e => e.NombreEstado, "IX_Estado").IsUnique();

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdTipoEstado)
                .HasDefaultValue((short)2)
                .HasColumnName("idTipoEstado");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreEstado)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdTipoEstadoNavigation).WithMany(p => p.Estados)
                .HasForeignKey(d => d.IdTipoEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estado_Estado");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.IdExpediente);

            entity.ToTable("Expediente");

            entity.HasIndex(e => e.NumExpediente, "IX_Expediente").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEstadoActual).HasColumnType("datetime");
            entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdArchivo).HasMaxLength(50);
            entity.Property(e => e.IdUsuarioAsignado)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NumExpediente)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Departamento");

            entity.HasOne(d => d.IdEntidadRepresentanteNavigation).WithMany(p => p.ExpedienteIdEntidadRepresentanteNavigations)
                .HasForeignKey(d => d.IdEntidadRepresentante)
                .HasConstraintName("FK_Expediente_EntidadRepresentante");

            entity.HasOne(d => d.IdEntidadResponsableNavigation).WithMany(p => p.ExpedienteIdEntidadResponsableNavigations)
                .HasForeignKey(d => d.IdEntidadResponsable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_EntidadResponsable");

            entity.HasOne(d => d.IdEntidadSolicitanteNavigation).WithMany(p => p.ExpedienteIdEntidadSolicitanteNavigations)
                .HasForeignKey(d => d.IdEntidadSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Entidad");

            entity.HasOne(d => d.IdEstadoActualNavigation).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.IdEstadoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Estado");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Municipio");

            entity.HasOne(d => d.IdSubTramiteNavigation).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.IdSubTramite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_SubTramite");

            entity.HasOne(d => d.IdTramiteNavigation).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.IdTramite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Tramite");
        });

        modelBuilder.Entity<ExpedienteArchivoAdjunto>(entity =>
        {
            entity.HasKey(e => e.IdExpedienteArchivoAdjunto);

            entity.ToTable("ExpedienteArchivoAdjunto");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.Extension).HasMaxLength(10);
            entity.Property(e => e.FechaCarga)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NombreArchivo)
                .IsRequired()
                .HasMaxLength(256)
                .HasComment("Nombre del archivo");
            entity.Property(e => e.Tamaño).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.ExpedienteArchivoAdjuntos)
                .HasForeignKey(d => d.IdExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Adjunto");
        });

        modelBuilder.Entity<ExpedienteDetalle>(entity =>
        {
            entity.HasKey(e => e.IdExpedienteDetalle);

            entity.ToTable("ExpedienteDetalle");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaAsignacion).HasColumnType("datetime");
            entity.Property(e => e.FechaConfirmacionAsignacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioAsignado).HasMaxLength(50);
            entity.Property(e => e.IdUsuarioRecibe).HasMaxLength(50);
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdEstadoActualNavigation).WithMany(p => p.ExpedienteDetalleIdEstadoActualNavigations)
                .HasForeignKey(d => d.IdEstadoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteDetalle_EstadoActual");

            entity.HasOne(d => d.IdEstadoAsignadoNavigation).WithMany(p => p.ExpedienteDetalleIdEstadoAsignadoNavigations)
                .HasForeignKey(d => d.IdEstadoAsignado)
                .HasConstraintName("FK_ExpedienteDetalle_EstadoAsignado");

            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.ExpedienteDetalles)
                .HasForeignKey(d => d.IdExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteDetalle_Expediente");
        });

        modelBuilder.Entity<ExpedienteNotificacion>(entity =>
        {
            entity.HasKey(e => e.IdExpedienteNotificacion);

            entity.ToTable("ExpedienteNotificacion");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioNotifica)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Mensaje).IsRequired();
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdEstadoActualNavigation).WithMany(p => p.ExpedienteNotificacions)
                .HasForeignKey(d => d.IdEstadoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteNotificacion_Estado");

            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.ExpedienteNotificacions)
                .HasForeignKey(d => d.IdExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteNotificacion_Expediente");
        });

        modelBuilder.Entity<ExpedienteRequisito>(entity =>
        {
            entity.HasKey(e => e.IdExpedienteRequisito);

            entity.ToTable("ExpedienteRequisito");

            entity.HasIndex(e => new { e.IdExpediente, e.IdRequisito }, "IX_ExpedienteRequisito").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.ExpedienteRequisitos)
                .HasForeignKey(d => d.IdExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteRequisito_Expediente");

            entity.HasOne(d => d.IdRequisitoNavigation).WithMany(p => p.ExpedienteRequisitos)
                .HasForeignKey(d => d.IdRequisito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpedienteRequisito_Requisito");
        });

        modelBuilder.Entity<Flujo>(entity =>
        {
            entity.HasKey(e => e.IdFlujo);

            entity.ToTable("Flujo");

            entity.HasIndex(e => e.NombreFlujo, "IX_Flujo").IsUnique();

            entity.HasIndex(e => new { e.IdSubTramite, e.Activo }, "IX_Flujo_SubTramite");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NombreFlujo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TiempoMin).HasDefaultValue(0);

            entity.HasOne(d => d.IdEstadoFinalNavigation).WithMany(p => p.FlujoIdEstadoFinalNavigations)
                .HasForeignKey(d => d.IdEstadoFinal)
                .HasConstraintName("FK_Flujo_EstadoFinal");

            entity.HasOne(d => d.IdEstadoInicialNavigation).WithMany(p => p.FlujoIdEstadoInicialNavigations)
                .HasForeignKey(d => d.IdEstadoInicial)
                .HasConstraintName("FK_Flujo_EstadoInicial");

            entity.HasOne(d => d.IdSubTramiteNavigation).WithMany(p => p.Flujos)
                .HasForeignKey(d => d.IdSubTramite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flujo_SubTramite");
        });

        modelBuilder.Entity<FlujoBloque>(entity =>
        {
            entity.HasKey(e => e.IdBloque);

            entity.ToTable("FlujoBloque");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NombreBloque)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdFlujoNavigation).WithMany(p => p.FlujoBloques)
                .HasForeignKey(d => d.IdFlujo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlujoBloque_Flujo");

            entity.HasOne(d => d.IdUnidadNavigation).WithMany(p => p.FlujoBloques)
                .HasForeignKey(d => d.IdUnidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlujoBloque_Unidad");

            entity.HasOne(d => d.IdEstadoFinalNavigation).WithMany(p => p.BloqueIdEstadoFinalNavigations)
            .HasForeignKey(d => d.IdEstadoFinal)
            .HasConstraintName("FK_FlujoBloque_EstadoFinal");

            entity.HasOne(d => d.IdEstadoInicialNavigation).WithMany(p => p.BloqueIdEstadoInicialNavigations)
                .HasForeignKey(d => d.IdEstadoInicial)
                .HasConstraintName("FK_FlujoBloque_EstadoInicial");
        });

        modelBuilder.Entity<FlujoTransicion>(entity =>
        {
            entity.HasKey(e => e.IdTransicion).HasName("PK_Transicion");

            entity.ToTable("FlujoTransicion");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");

            entity.HasOne(d => d.IdBloqueNavigation).WithMany(p => p.FlujoTransicions)
                .HasForeignKey(d => d.IdBloque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlujoTransicion_FlujoBloque");

            entity.HasOne(d => d.IdEstadoFinalNavigation).WithMany(p => p.FlujoTransicionIdEstadoFinalNavigations)
                .HasForeignKey(d => d.IdEstadoFinal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlujoTransicion_EstadoFinal");

            entity.HasOne(d => d.IdEstadoInicialNavigation).WithMany(p => p.FlujoTransicionIdEstadoInicialNavigations)
                .HasForeignKey(d => d.IdEstadoInicial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlujoTransicion_EstadoInicial");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.IdMunicipio).HasName("PK__Municipi__61005978A10A394E");

            entity.ToTable("Municipio");

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Municipios)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK_Municipio_Departamento");
        });

        modelBuilder.Entity<Requisito>(entity =>
        {
            entity.HasKey(e => e.IdRequisito);

            entity.ToTable("Requisito");

            entity.HasIndex(e => e.NombreRequisito, "IX_Requisito").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreRequisito)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Observaciones).HasMaxLength(2000);
        });

        modelBuilder.Entity<Resolucion>(entity =>
        {
            entity.HasKey(e => e.IdResolucion);

            entity.ToTable("Resolucion");

            entity.HasIndex(e => e.NumeroResolucion, "IX_Resolucion").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioResolucion)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Justificacion).IsRequired();
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
            entity.Property(e => e.NumeroResolucion)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasOne(d => d.IdExpedienteNavigation).WithMany(p => p.Resolucion)
              .HasForeignKey(d => d.IdExpediente)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Resolucion_Expediente");
        });

        modelBuilder.Entity<RoleVentana>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleVent__3214EC079001E89B");

            entity.ToTable("RoleVentana");

            entity.Property(e => e.RoleId)
                .IsRequired()
                .HasMaxLength(450);
            entity.Property(e => e.Ver).HasColumnName("ver");

            entity.HasOne(rv => rv.Role)
            .WithMany()
            .HasForeignKey(rv => rv.RoleId);

            entity.HasOne(rv => rv.Ventana)
                .WithMany(v => v.RoleVentanas)
                .HasForeignKey(rv => rv.VentanaId);
        });

        modelBuilder.Entity<SubTramite>(entity =>
        {
            entity.HasKey(e => e.IdSubTramite);

            entity.ToTable("SubTramite");

            entity.HasIndex(e => e.Codigo, "IX_SubTramite_Codigo").IsUnique();

            entity.HasIndex(e => e.NombreSubTramite, "IX_SubTramite_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.CodigoFinanzas).HasMaxLength(10);
            entity.Property(e => e.Costo)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreSubTramite)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.IdTramiteNavigation).WithMany(p => p.SubTramites)
                .HasForeignKey(d => d.IdTramite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubTramite_Tramite1");
        });

        modelBuilder.Entity<SubTramiteRequisito>(entity =>
        {
            entity.HasKey(e => e.IdRequisitoSubTramite);

            entity.ToTable("SubTramiteRequisito");

            entity.HasIndex(e => new { e.IdRequisito, e.IdSubtramite }, "IX_SubTramiteRequisito").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdRequisitoNavigation).WithMany(p => p.SubTramiteRequisitos)
                .HasForeignKey(d => d.IdRequisito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubTramiteRequisito_Requisito");

            entity.HasOne(d => d.IdSubtramiteNavigation).WithMany(p => p.SubTramiteRequisitos)
                .HasForeignKey(d => d.IdSubtramite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubTramiteRequisito_SubTramite");
        });

        modelBuilder.Entity<TipoEntidad>(entity =>
        {
            entity.HasKey(e => e.IdTipoEntidad);

            entity.ToTable("TipoEntidad");

            entity.HasIndex(e => e.NombreTipoEntidad, "IX_TipoEntidad_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreTipoEntidad)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TipoEstado>(entity =>
        {
            entity.HasKey(e => e.IdTipoEstado);

            entity.ToTable("TipoEstado");

            entity.HasIndex(e => e.NombreTipoEstado, "IX_TipoEstado_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreTipoEstado)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.HasKey(e => e.IdTramite);

            entity.ToTable("Tramite");

            entity.HasIndex(e => e.Codigo, "IX_Tramite_Codigo").IsUnique();

            entity.HasIndex(e => e.NombreTramite, "IX_Tramite_Nombre").IsUnique();

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.DivisionTramites)
                .HasForeignKey(d => d.IdDivision)
                .HasConstraintName("FK_Tramite_Division");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreTramite)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Unidad>(entity =>
        {
            entity.HasKey(e => e.IdUnidad);

            entity.ToTable("Unidad");

            entity.HasIndex(e => e.NombreUnidad, "IX_Unidad_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreUnidad)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.IdDivisionDepartamentoNavigation).WithMany(p => p.Unidads)
                .HasForeignKey(d => d.IdDivisionDepartamento)
                .HasConstraintName("FK_Unidad_DivisionDepartamento");
        });

        modelBuilder.Entity<Ventana>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ventana__3214EC07EEBEFA6F");

            entity.ToTable("Ventana");

            entity.HasIndex(e => e.Nombre, "IX_Ventana_Nombre").IsUnique();

            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.ModificadoPor).HasMaxLength(256);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
