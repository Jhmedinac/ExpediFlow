using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class TipoEstado
{
    [Required(ErrorMessage = "El campo IdTipoEstado es requerido.")]
    [DisplayName("ID Tipo Estado")]
    public short IdTipoEstado { get; set; }

    [Required(ErrorMessage = "El campo NombreTipoEstado es requerido.")]
    [StringLength(100, ErrorMessage = "El Nombre Tipo Estado no puede exceder los 100 caracteres.")]
    [DisplayName("Nombre Tipo Estado")]
    public string NombreTipoEstado { get; set; }

    [Required(ErrorMessage = "El campo Notificar es requerido.")]
    [DisplayName("Notificar")]
    public bool Notificar { get; set; }

    [Required(ErrorMessage = "El campo Activo es requerido.")]
    [DisplayName("Activo")]
    public bool Activo { get; set; }

    
    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

    
    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

   
    [StringLength(50, ErrorMessage = "El campo CreadoPor no puede exceder los 50 caracteres.")]
    [DisplayName("Creado Por")]
    public string CreadoPor { get; set; }

   
    [StringLength(50, ErrorMessage = "El campo ModificadoPor no puede exceder los 50 caracteres.")]
    [DisplayName("Modificado Por")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<Estado> Estados { get; set; } = new List<Estado>();
}
