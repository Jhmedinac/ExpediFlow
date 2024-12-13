using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class SubTramite
{
    [Key]
    public int IdSubTramite { get; set; }

    [Required(ErrorMessage = "El campo Nombre del Sub-Trámite es obligatorio.")]
    [Display(Name = "Nombre del Sub-Trámite")]
    [StringLength(100)]
    public string NombreSubTramite { get; set; }

    [Required(ErrorMessage = "El campo Trámite es obligatorio.")]
    [Display(Name = "Trámite")]
    public int IdTramite { get; set; }

    [Required(ErrorMessage = "El campo Código es obligatorio.")]
    [Display(Name = "Código")]
    [StringLength(5)]
    public string Codigo { get; set; }

    [Display(Name = "Costo")]
    [Range(0, 99999999.99, ErrorMessage = "El valor de Costo debe estar entre 0 y 99,999,999.99.")]
    public decimal? Costo { get; set; }

    [Display(Name = "Código de Finanzas")]
    [StringLength(10)]
    public string CodigoFinanzas { get; set; }

    [Required(ErrorMessage = "El campo Activo es obligatorio.")]
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

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual ICollection<Flujo> Flujos { get; set; } = new List<Flujo>();

   
    [Display(Name = "Trámite")]

    public virtual Tramite IdTramiteNavigation { get; set; }

    public virtual ICollection<SubTramiteRequisito> SubTramiteRequisitos { get; set; } = new List<SubTramiteRequisito>();
}
