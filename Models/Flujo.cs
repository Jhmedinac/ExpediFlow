using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Flujo
{
    [Key]
    public int IdFlujo { get; set; }

    [Required(ErrorMessage = "El campo Nombre de Flujo es obligatorio")]
    [Display(Name = "Nombre de Flujo")]
    [StringLength(50)]
    public string NombreFlujo { get; set; }

    [Required(ErrorMessage = "El campo SubTrámite es obligatorio")]
    [Display(Name = "SubTrámite")]
    public int IdSubTramite { get; set; }

    [Display(Name = "Descripción")]
    public string Descipcion { get; set; }

    [Display(Name = "Estado Inicial")]
    public int? IdEstadoInicial { get; set; }

    [Display(Name = "Estado Final")]
    public int? IdEstadoFinal { get; set; }

    [Display(Name = "Tiempo Mínimo")]
    public int? TiempoMin { get; set; }

    [Display(Name = "Tiempo Máximo")]
    public int? TiempoMax { get; set; }

    [Required(ErrorMessage = "El campo Activo es obligatorio")]
    [Display(Name = "Activo")]
    public bool Activo { get; set; }

  
    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

   
    [Display(Name = "Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

   
    [Display(Name = "Creado Por")]
    [StringLength(50)]
    public string CreadoPor { get; set; }
 
    [Display(Name = "Modificado Por")]
    [StringLength(50)]
    public string ModificadoPor { get; set; }

    public virtual ICollection<FlujoBloque> FlujoBloques { get; set; } = new List<FlujoBloque>();
    [Display(Name = "Estado Final")]
    public virtual Estado IdEstadoFinalNavigation { get; set; }
    [Display(Name = "EstadoInicial")]
    public virtual Estado IdEstadoInicialNavigation { get; set; }
    [Display(Name = "Sub-Tramite")]
    public virtual SubTramite IdSubTramiteNavigation { get; set; }
}
