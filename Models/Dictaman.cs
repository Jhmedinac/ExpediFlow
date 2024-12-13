using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class Dictaman
{
    [Key]
    public int IdDictamen { get; set; }

    [Required(ErrorMessage = "El campo 'IdExpediente' es obligatorio.")]
    [DisplayName("Expediente")]
    public int IdExpediente { get; set; }

    [Required(ErrorMessage = "El campo 'Número de Dictamen' es obligatorio.")]
    [StringLength(50, ErrorMessage = "El 'Número de Dictamen' no puede tener más de 50 caracteres.")]
    [DisplayName("Número de Dictamen")]
    public string NumeroDictamen { get; set; }

    [Required(ErrorMessage = "El campo 'Fecha de Dictamen' es obligatorio.")]
    [Column(TypeName = "date")]
    [DisplayName("Fecha")] // Aquí se agrega el nombre de visualización
    public DateOnly FechaDictamen { get; set; }

    [Required(ErrorMessage = "El campo 'Justificación' es obligatorio.")]
    [DisplayName("Justificación")]
    public string Justificacion { get; set; }

    [Required(ErrorMessage = "El campo 'ID del Usuario del Dictamen' es obligatorio.")]
    [StringLength(50, ErrorMessage = "El 'ID del Usuario del Dictamen' no puede tener más de 50 caracteres.")]
    [DisplayName("Generado Por")]
    public string IdUsuarioDictamen { get; set; }

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

    [ForeignKey("IdExpediente")]
    [DisplayName("Expediente")]
    public virtual Expediente IdExpedienteNavigation { get; set; }
}
