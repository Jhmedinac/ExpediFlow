using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class Ventana
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual ICollection<RoleVentana> RoleVentanas { get; set; } = new List<RoleVentana>();
}
