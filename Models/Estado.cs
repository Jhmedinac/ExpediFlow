using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class Estado
{
    [Key] 
    public int IdEstado { get; set; }

    [Required(ErrorMessage = "El nombre del estado es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre del estado no puede exceder los 50 caracteres.")]
    [DisplayName("Nombre del Estado")]
    public string NombreEstado { get; set; }

    [Required(ErrorMessage = "El tiempo mínimo es requerido.")]
    [DisplayName("Tiempo Mínimo")]
    public int TiempoMin { get; set; }

    [Required(ErrorMessage = "El tiempo máximo es requerido.")]
    [DisplayName("Tiempo Máximo")]
    public int TiempoMax { get; set; }

    [Required(ErrorMessage = "El tipo de estado es requerido.")]
    [DisplayName("Tipo de Estado")]
    public short IdTipoEstado { get; set; }

    [DisplayName("Activo")]
    public bool Activo { get; set; }

    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

    [DisplayName("Creado Por")]
    [StringLength(50, ErrorMessage = "El creador no puede exceder los 50 caracteres.")]
    public string CreadoPor { get; set; }

    [DisplayName("Modificado Por")]
    [StringLength(50, ErrorMessage = "El modificador no puede exceder los 50 caracteres.")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<ExpedienteDetalle> ExpedienteDetalleIdEstadoActualNavigations { get; set; } = new List<ExpedienteDetalle>();

    public virtual ICollection<ExpedienteDetalle> ExpedienteDetalleIdEstadoAsignadoNavigations { get; set; } = new List<ExpedienteDetalle>();

    public virtual ICollection<ExpedienteNotificacion> ExpedienteNotificacions { get; set; } = new List<ExpedienteNotificacion>();

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual ICollection<Flujo> FlujoIdEstadoFinalNavigations { get; set; } = new List<Flujo>();

    public virtual ICollection<Flujo> FlujoIdEstadoInicialNavigations { get; set; } = new List<Flujo>();

    public virtual ICollection<FlujoTransicion> FlujoTransicionIdEstadoFinalNavigations { get; set; } = new List<FlujoTransicion>();

    public virtual ICollection<FlujoTransicion> FlujoTransicionIdEstadoInicialNavigations { get; set; } = new List<FlujoTransicion>();
    [DisplayName("Tipo de Estado")]
    public virtual TipoEstado IdTipoEstadoNavigation { get; set; }

    public virtual ICollection<FlujoBloque> BloqueIdEstadoFinalNavigations { get; set; } = new List<FlujoBloque>();

    public virtual ICollection<FlujoBloque> BloqueIdEstadoInicialNavigations { get; set; } = new List<FlujoBloque>();
}
