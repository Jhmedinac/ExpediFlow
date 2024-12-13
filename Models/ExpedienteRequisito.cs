using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class ExpedienteRequisito
{
    public int IdExpedienteRequisito { get; set; }

    public int IdExpediente { get; set; }

    public int IdRequisito { get; set; }

    public bool Cumple { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual Expediente IdExpedienteNavigation { get; set; }

    public virtual Requisito IdRequisitoNavigation { get; set; }
}
