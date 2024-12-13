using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class ExpedienteArchivoAdjunto
{
    public int IdExpedienteArchivoAdjunto { get; set; }

    public int IdExpediente { get; set; }

    public DateTime FechaCarga { get; set; }

    /// <summary>
    /// Nombre del archivo
    /// </summary>
    public string NombreArchivo { get; set; }

    public string Ruta { get; set; }

    public decimal? Tamaño { get; set; }

    public string Extension { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual Expediente IdExpedienteNavigation { get; set; }
}
