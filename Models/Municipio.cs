using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Municipio
{
    public int IdMunicipio { get; set; }

    [DisplayName("Nombre")]
    [Required(ErrorMessage = "El Nombre es obligatorio.")]
    public string Nombre { get; set; }

    [DisplayName("Departamento")]
   // [Required(ErrorMessage = "El Departamento es obligatorio.")]
    public int? IdDepartamento { get; set; }

    public virtual ICollection<Entidad> Entidads { get; set; } = new List<Entidad>();

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    [DisplayName("Departamento")]
    public virtual Departamento IdDepartamentoNavigation { get; set; }
}
