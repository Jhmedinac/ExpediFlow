using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Unidad
{
    [Key]
    public int IdUnidad { get; set; }

    [Required(ErrorMessage = "El campo Nombre Unidad es requerido.")]
    [Display(Name = "Nombre de la Unidad")]
    [StringLength(100, ErrorMessage = "El campo Nombre Unidad no puede exceder los 100 caracteres.")]
    public string NombreUnidad { get; set; }

    [Display(Name = "División-Departamento")]
    public int? IdDivisionDepartamento { get; set; }

    [Required(ErrorMessage = "El campo Activo es requerido.")]
    [Display(Name = "Activo")]
    public bool Activo { get; set; }

   
    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

  
    [Display(Name = "Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

   
    [Display(Name = "Creado por")]
    [StringLength(50, ErrorMessage = "El campo Creado Por no puede exceder los 50 caracteres.")]
    public string CreadoPor { get; set; }

    
    [Display(Name = "Modificado por")]
    [StringLength(50, ErrorMessage = "El campo Modificado Por no puede exceder los 50 caracteres.")]
    public string ModificadoPor { get; set; }

    //public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    public virtual ICollection<FlujoBloque> FlujoBloques { get; set; } = new List<FlujoBloque>();

    [Display(Name = "División-Departamento")]
    public virtual DivisionDepartamento IdDivisionDepartamentoNavigation { get; set; }
}
