using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class RoleVentana
{
    public int Id { get; set; }

    public string RoleId { get; set; }

    public int VentanaId { get; set; }

    public bool Ver { get; set; }

    public bool Crear { get; set; }

    public bool Editar { get; set; }

    public bool Eliminar { get; set; }

    public virtual IdentityRole Role { get; set; }

    public virtual Ventana Ventana { get; set; }
}
