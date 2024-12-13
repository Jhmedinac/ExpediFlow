using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class ExpedienteNotificacion
{
    public int IdExpedienteNotificacion { get; set; }

    public int IdExpediente { get; set; }

    public int IdEstadoActual { get; set; }

    public string IdUsuarioNotifica { get; set; }

    public string Email { get; set; }

    public string Mensaje { get; set; }

    public DateTime FechaEnvio { get; set; }

    public bool Enviado { get; set; }

    public bool Leido { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual Estado IdEstadoActualNavigation { get; set; }

    public virtual Expediente IdExpedienteNavigation { get; set; }
}
