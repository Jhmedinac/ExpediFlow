using System;
using System.Collections.Generic;

namespace ExpediFlow.Models;

public partial class FlujoTransicion
{
    public int IdTransicion { get; set; }

    public int IdBloque { get; set; }

    public int IdEstadoInicial { get; set; }

    public int IdEstadoFinal { get; set; }

    public bool Enviar { get; set; }

    public bool Recibir { get; set; }

    public int Orden { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string CreadoPor { get; set; }

    public string ModificadoPor { get; set; }

    public virtual FlujoBloque IdBloqueNavigation { get; set; }

    public virtual Estado IdEstadoFinalNavigation { get; set; }

    public virtual Estado IdEstadoInicialNavigation { get; set; }
}
