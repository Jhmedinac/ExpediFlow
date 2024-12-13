using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class FlujoBloque
{
    public int IdBloque { get; set; }

    public int IdFlujo { get; set; }

    public string NombreBloque { get; set; }

    public int IdUnidad { get; set; }

    [Display(Name = "Estado Inicial")]
    public int? IdEstadoInicial { get; set; }

    [Display(Name = "Estado Final")]
    public int? IdEstadoFinal { get; set; }

    public int Orden { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual ICollection<FlujoTransicion> FlujoTransicions { get; set; } = new List<FlujoTransicion>();

    public virtual Flujo IdFlujoNavigation { get; set; }

    public virtual Unidad IdUnidadNavigation { get; set; }

    [Display(Name = "Estado Final")]
    public virtual Estado IdEstadoFinalNavigation { get; set; }
    [Display(Name = "EstadoInicial")]
    public virtual Estado IdEstadoInicialNavigation { get; set; }
}
