using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Requisito
{
    [Key]
    public int IdRequisito { get; set; }

    [Required(ErrorMessage = "El campo Nombre de Requisito es obligatorio.")]
    [Display(Name = "Nombre de Requisito")]
    [StringLength(100, ErrorMessage = "El campo Nombre de Requisito no puede exceder los 100 caracteres.")]
    public string NombreRequisito { get; set; }

    [Display(Name = "Observaciones")]
    [StringLength(500, ErrorMessage = "El campo Observaciones no puede exceder los 500 caracteres.")]
    public string Observaciones { get; set; }

    [Required(ErrorMessage = "El campo Activo es obligatorio.")]
    [Display(Name = "Activo")]
    public bool Activo { get; set; }
    [Display(Name = "Fecha Creacion")]
    public DateTime FechaCreacion { get; set; }
    [Display(Name = "Fecha Modificacion")]
    public DateTime FechaModificacion { get; set; }
    [Display(Name = "Creado Por")]
    public string CreadoPor { get; set; }
    [Display(Name = "Modificado Por")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<ExpedienteRequisito> ExpedienteRequisitos { get; set; } = new List<ExpedienteRequisito>();

    public virtual ICollection<SubTramiteRequisito> SubTramiteRequisitos { get; set; } = new List<SubTramiteRequisito>();
}
