using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class Tramite
{
    [Key]
    [Required(ErrorMessage = "El Id del trámite es obligatorio.")]
    public int IdTramite { get; set; }

    [Required(ErrorMessage = "El nombre del trámite es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del trámite no puede superar los 100 caracteres.")]
    [DisplayName("Nombre del Trámite")]
    public string NombreTramite { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código no puede superar los 10 caracteres.")]
    [DisplayName("Código")]
    public string Codigo { get; set; }

    [DisplayName("División")]
    [Required(ErrorMessage = "La división es obligatoria.")]
    public int IdDivision { get; set; }

    [DisplayName("Activo")]
    [Required(ErrorMessage = "El estado activo es obligatorio.")]
    public bool Activo { get; set; }

    
    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

   
    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

   
    [StringLength(50, ErrorMessage = "El nombre del creador no puede superar los 50 caracteres.")]
    [DisplayName("Creado Por")]
    public string CreadoPor { get; set; }

   
    [StringLength(50, ErrorMessage = "El nombre del modificador no puede superar los 50 caracteres.")]
    [DisplayName("Modificado Por")]
    public string ModificadoPor { get; set; }

    [DisplayName("División")]
    public virtual Division IdDivisionNavigation { get; set; }

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();


    public virtual ICollection<SubTramite> SubTramites { get; set; } = new List<SubTramite>();

}
