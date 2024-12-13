using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class DivisionDepartamento
{
    [Key]
    public int IdDivisionDepartamento { get; set; }

    [Required(ErrorMessage = "El campo Nombre del Departamento es obligatorio.")]
    [DisplayName("Nombre del Departamento")]
    [StringLength(50, ErrorMessage = "El Nombre del Departamento no puede exceder los 50 caracteres.")]
    public string NombreDepartamento { get; set; }

    [DisplayName("División")]
    public int? IdDivision { get; set; }

    [Required(ErrorMessage = "El campo Activo es obligatorio.")]
    public bool Activo { get; set; }

   
    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

   
    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

    
    [DisplayName("Creado Por")]
    [StringLength(50, ErrorMessage = "El campo Creado Por no puede exceder los 50 caracteres.")]
    public string CreadoPor { get; set; }

   
    [DisplayName("Modificado Por")]
    [StringLength(50, ErrorMessage = "El campo Modificado Por no puede exceder los 50 caracteres.")]
    public string ModificadoPor { get; set; }

    [DisplayName("División")]
    public virtual Division IdDivisionNavigation { get; set; }

    public virtual ICollection<Unidad> Unidads { get; set; } = new List<Unidad>();
}
