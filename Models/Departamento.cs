using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Departamento
{
    [Key]
    public int IdDepartamento { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")] // Campo requerido
    [StringLength(100, ErrorMessage = "El nombre no puede exceder de 100 caracteres")] // Longitud máxima de 100 caracteres
    public string Nombre { get; set; }

    public virtual ICollection<Entidad> Entidads { get; set; } = new List<Entidad>();

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
