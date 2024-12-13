using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.Models;

public partial class Expediente
{
    [Key]
    public int IdExpediente { get; set; }

    [Required(ErrorMessage = "El Número de Expediente es requerido.")]
    [Display(Name = "Número de Expediente")]
    public string NumExpediente { get; set; }

    [Required(ErrorMessage = "El Trámite es requerido.")]
    [Display(Name = "Trámite")]
    public int IdTramite { get; set; }

    [Required(ErrorMessage = "El Sub-Trámite es requerido.")]
    [Display(Name = "Sub-Trámite")]
    public int IdSubTramite { get; set; }

    [Required(ErrorMessage = "El Sub-Trámite es requerido.")]
    [Display(Name = "Solicitante")]
    public int IdEntidadSolicitante { get; set; }

    [Required(ErrorMessage = "La Entidad Responsable es requerido.")]
    [Display(Name = "Responsable")]
    public int IdEntidadResponsable { get; set; }

    [Display(Name = "Apoderado")]
    public int? IdEntidadRepresentante { get; set; }

    [Required(ErrorMessage = "La Fecha de Ingreso es requerido.")]
    [Display(Name = "Fecha de Ingreso")]
    public DateOnly FechaIngreso { get; set; }

    [Required(ErrorMessage = "El Departamento es requerido.")]
    [Display(Name = "Departamento")]
    public int IdDepartamento { get; set; }

    [Required(ErrorMessage = "El Municipio es requerido.")]
    [Display(Name = "Municipio")]
    public int IdMunicipio { get; set; }

    [Required(ErrorMessage = "El Estado Actual es requerido.")]
    [Display(Name = "Estado Actual")]
    public int IdEstadoActual { get; set; }

    [Required(ErrorMessage = "La Fecha del Estado Actual es requerido.")]
    [Display(Name = "Fecha del Estado Actual")]
    public DateTime FechaEstadoActual { get; set; }

    [Required(ErrorMessage = "El Usuario Asignado es requerido.")]
    [Display(Name = "Usuario Asignado")]
    public string IdUsuarioAsignado { get; set; }

    [Display(Name = "Resolución")]
    public int? IdResolucion { get; set; }

    [Display(Name = "Dictamen")]
    public int? IdDictamen { get; set; }

    [Display(Name = "Archivo")]
    public string IdArchivo { get; set; }

    [Display(Name = "Observaciones")]
    public string Observaciones { get; set; }

    [Required(ErrorMessage = "El campo activo es requerido.")]
    [Display(Name = "Activo")]
    public bool Activo { get; set; }

  
    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

    
    [Display(Name = "Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

    
    [Display(Name = "Creado Por")]
    public string CreadoPor { get; set; }

    
    [Display(Name = "Modificado Por")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<Dictaman> Dictamen { get; set; } = new List<Dictaman>();
    public virtual ICollection<Resolucion> Resolucion { get; set; } = new List<Resolucion>();

    public virtual ICollection<ExpedienteArchivoAdjunto> ExpedienteArchivoAdjuntos { get; set; } = new List<ExpedienteArchivoAdjunto>();

    public virtual ICollection<ExpedienteDetalle> ExpedienteDetalles { get; set; } = new List<ExpedienteDetalle>();

    public virtual ICollection<ExpedienteNotificacion> ExpedienteNotificacions { get; set; } = new List<ExpedienteNotificacion>();

    public virtual ICollection<ExpedienteRequisito> ExpedienteRequisitos { get; set; } = new List<ExpedienteRequisito>();

    [Display(Name = "Departamento")]
    public virtual Departamento IdDepartamentoNavigation { get; set; }
    [Display(Name = "Entidad Representante")]
    public virtual Entidad IdEntidadRepresentanteNavigation { get; set; }
    [Display(Name = "Entidad Responsable")]
    public virtual Entidad IdEntidadResponsableNavigation { get; set; }
    [Display(Name = "Entidad Solicitante")]
    public virtual Entidad IdEntidadSolicitanteNavigation { get; set; }
    [Display(Name = "Estado Actual")]
    public virtual Estado IdEstadoActualNavigation { get; set; }
    [Display(Name = "Municipio")]
    public virtual Municipio IdMunicipioNavigation { get; set; }
    [Display(Name = "SubTramite")]
    public virtual SubTramite IdSubTramiteNavigation { get; set; }
    [Display(Name = "Tramite")]
    public virtual Tramite IdTramiteNavigation { get; set; }
}
