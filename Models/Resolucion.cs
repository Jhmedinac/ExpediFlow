using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpediFlow.Models;

public partial class Resolucion
{
    [Key]
    [Display(Name = "Id")]
    public int IdResolucion { get; set; }

    [Required(ErrorMessage = "El ID de Expediente es requerido.")]
    [Display(Name = "Expediente")]
    public int IdExpediente { get; set; }

    [Required(ErrorMessage = "El Número de Resolución es requerido.")]
    [Display(Name = "Número de Resolución")]
    [StringLength(50, ErrorMessage = "El Número de Resolución no puede exceder los 50 caracteres.")]
    public string NumeroResolucion { get; set; }

    [Required(ErrorMessage = "La Fecha de Resolución es requerida.")]
    [Display(Name = "Fecha de Resolución")]
    public DateOnly FechaResolucion { get; set; }

    [Required(ErrorMessage = "La Justificación es requerida.")]
    [Display(Name = "Justificación")]
    public string Justificacion { get; set; }

    [Required(ErrorMessage = "El ID del Usuario de Resolución es requerido.")]
    [Display(Name = "Generada Por")]
    [StringLength(50, ErrorMessage = "El ID de Usuario de Resolución no puede exceder los 50 caracteres.")]
    public string IdUsuarioResolucion { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; }
    [Display(Name = "Fecha Creacion")]
    public DateTime FechaCreacion { get; set; }
    [Display(Name = "Fecha Modificacion")]
    public DateTime FechaModificacion { get; set; }
    [Display(Name = "Creado Por")]
    public string CreadoPor { get; set; }
    [Display(Name = "Modificado Por")]
    public string ModificadoPor { get; set; }

    [ForeignKey("IdExpediente")]
    [DisplayName("Expediente")]
    public virtual Expediente IdExpedienteNavigation { get; set; }
}
