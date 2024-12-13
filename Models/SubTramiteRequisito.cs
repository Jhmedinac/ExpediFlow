using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class SubTramiteRequisito
{
    [Key]
    [Display(Name = "ID")]
    public int IdRequisitoSubTramite { get; set; }

    [Required(ErrorMessage = "El ID del SubTrámite es requerido.")]
    [Display(Name = "SubTrámite")]
    public int IdSubtramite { get; set; }

    [Required(ErrorMessage = "El ID del Requisito es requerido.")]
    [Display(Name = "Requisito")]
    public int IdRequisito { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; }

    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

    [Display(Name = "Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }
  
    [StringLength(50, ErrorMessage = "El campo Creado Por no puede tener más de 50 caracteres.")]
    [Display(Name = "Creado Por")]
    public string CreadoPor { get; set; }
    
    [StringLength(50, ErrorMessage = "El campo Modificado Por no puede tener más de 50 caracteres.")]
    [Display(Name = "Modificado Por")]
    public string ModificadoPor { get; set; }
    [Display(Name = "Requisito")]
    public virtual Requisito IdRequisitoNavigation { get; set; }
    [Display(Name = "Sub-tramite")]
    public virtual SubTramite IdSubtramiteNavigation { get; set; }
}
