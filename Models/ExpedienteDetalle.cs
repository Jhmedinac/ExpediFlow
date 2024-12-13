using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class ExpedienteDetalle
{
    public int IdExpedienteDetalle { get; set; }

    public int IdExpediente { get; set; }

    public int IdEstadoActual { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public string IdUsuarioRecibe { get; set; }

    public int? IdEstadoAsignado { get; set; }

    public string IdUsuarioAsignado { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public DateTime? FechaConfirmacionAsignacion { get; set; }

    public string Comentarios { get; set; }

    public bool IndReasignado { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual Estado IdEstadoActualNavigation { get; set; }

    public virtual Estado IdEstadoAsignadoNavigation { get; set; }

    public virtual Expediente IdExpedienteNavigation { get; set; }
}
