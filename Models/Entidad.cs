using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExpediFlow.Models;

public partial class Entidad
{
    public int IdEntidad { get; set; }

    /// <summary>
    /// 1: Persona Natural, 2: Institución, 3: Apoderado
    /// </summary>
    [Required(ErrorMessage = "El tipo de entidad es requerido.")]
    [DisplayName("Tipo de Entidad")]
    public int IdTipoEntidad { get; set; }

    [StringLength(20, ErrorMessage = "El DNI no puede exceder los 20 caracteres.")]
    [DisplayName("No. de Identidad")]
    public string Dni { get; set; }

    [StringLength(20, ErrorMessage = "El RTN no puede exceder los 20 caracteres.")]
    [DisplayName("RTN")]
    public string Rtn { get; set; }

    [StringLength(10, ErrorMessage = "El código no puede exceder los 10 caracteres.")]
    [DisplayName("Código")]
    public string Codigo { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [DisplayName("Nombre")]
    public string Nombre { get; set; }

    [StringLength(255, ErrorMessage = "La dirección no puede exceder los 255 caracteres.")]
    [DisplayName("Dirección")]
    public string Direccion { get; set; }

    [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
    [DisplayName("Teléfono")]
    public string Telefono { get; set; }

    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [DisplayName("Correo Electrónico")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El departamento es requerido.")]
    [DisplayName("Departamento")]
    public int IdDepartamento { get; set; }

    [Required(ErrorMessage = "El municipio es requerido.")]
    [DisplayName("Municipio")]
    public int IdMunicipio { get; set; }

    [StringLength(int.MaxValue, ErrorMessage = "Las observaciones no pueden exceder los 2147483647 caracteres.")]
    [DisplayName("Observaciones")]
    public string Observaciones { get; set; }

    public bool Activo { get; set; }

    
    [DisplayName("Fecha de Creación")]
    public DateTime FechaCreacion { get; set; }

   
    [DisplayName("Fecha de Modificación")]
    public DateTime FechaModificacion { get; set; }

    [StringLength(50, ErrorMessage = "El campo Creado Por no puede exceder los 50 caracteres.")]
    [DisplayName("Creado Por")]
    public string CreadoPor { get; set; }

    [StringLength(50, ErrorMessage = "El campo Modificado Por no puede exceder los 50 caracteres.")]
    [DisplayName("Modificado Por")]
    public string ModificadoPor { get; set; }

    public virtual ICollection<Expediente> ExpedienteIdEntidadRepresentanteNavigations { get; set; } = new List<Expediente>();

    public virtual ICollection<Expediente> ExpedienteIdEntidadResponsableNavigations { get; set; } = new List<Expediente>();

    public virtual ICollection<Expediente> ExpedienteIdEntidadSolicitanteNavigations { get; set; } = new List<Expediente>();

    [DisplayName("Departamento")]
    public virtual Departamento IdDepartamentoNavigation { get; set; }
    [DisplayName("Municipio")]
    public virtual Municipio IdMunicipioNavigation { get; set; }
    [DisplayName("Tipo de Entidad")]
    public virtual TipoEntidad IdTipoEntidadNavigation { get; set; }
}


