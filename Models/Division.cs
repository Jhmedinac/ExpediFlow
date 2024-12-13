using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class Division
{
    [Key]
    public int IdDivision { get; set; }

    [Required(ErrorMessage = "El campo 'Nombre de la División' es obligatorio.")]
    [StringLength(100, ErrorMessage = "El 'Nombre de la División' no puede tener más de 100 caracteres.")]
    [DisplayName("Nombre de la División")]
    public string NombreDivision { get; set; }

    [Required(ErrorMessage = "El campo 'Activo' es obligatorio.")]
    [DisplayName("Activo")]
    public bool Activo { get; set; }

    
    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

    
    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

   
    [StringLength(50, ErrorMessage = "El 'Creado Por' no puede tener más de 50 caracteres.")]
    [DisplayName("Creado Por")]
    public string CreadoPor { get; set; }

    
    [StringLength(50, ErrorMessage = "El 'Modificado Por' no puede tener más de 50 caracteres.")]
    [DisplayName("Modificado Por")]
    public string ModificadoPor { get; set; }

    [DisplayName("Departamentos de la División")]
    public virtual ICollection<DivisionDepartamento> DivisionDepartamentos { get; set; } = new List<DivisionDepartamento>();

    [DisplayName("Trámite División")]
    public virtual ICollection<Tramite> DivisionTramites { get; set; } = new List<Tramite>();
}