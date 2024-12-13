using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class TipoEntidad
{
    [Key] // Anotación para indicar que esta propiedad es la clave primaria
    public int IdTipoEntidad { get; set; }

    [Required(ErrorMessage = "El nombre del tipo de entidad es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre del tipo de entidad no puede exceder los 50 caracteres.")]
    [DisplayName("Nombre Tipo Entidad")] // Nombre amigable para la interfaz de usuario
    public string NombreTipoEntidad { get; set; }

    [DisplayName("Activo")]
    [Required(ErrorMessage = "El estado activo es obligatorio.")]
    public bool Activo { get; set; }

    
    [DisplayName("Fecha Creacion")]
    public DateTime FechaCreacion { get; set; }

    [DisplayName("Fecha Modificacion")]
    public DateTime FechaModificacion { get; set; }

    [DisplayName("Creado Por")]
    [StringLength(50, ErrorMessage = "El nombre del creador no puede exceder los 50 caracteres.")]
    public string CreadoPor { get; set; }

    [DisplayName("Modificado Por")]
    [StringLength(50, ErrorMessage = "El nombre del modificador no puede exceder los 50 caracteres.")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<Entidad> Entidads { get; set; } = new List<Entidad>();
}
