﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace ExpediFlow.Models
{
    [Display(Name = "Empresa/Institución")]
    public partial class Empresa
    {
        public int IdEmpresa { get; set; }
        [Display(Name = "Nombre de la Empresa/Institución")]
        [Required(ErrorMessage = "El Nombre de la Empresa es obligatorio.")]
        public string NombreEmpresa { get; set; } = null!;

        [Display(Name = "Siglas")]
        public string Siglas { get; set; } = null!;

        [Display(Name = "Logo Tipo")]
        public byte[] Logo { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El Teléfono es obligatorio.")]
        public string Telefono { get; set; } = null!;
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Número de Registro Tributario")]
        public string Rtn { get; set; }
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }

        [DisplayName("Activo")]
        public bool Activo { get; set; }

        [DisplayName("Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [DisplayName("Fecha de Modificación")]
        public DateTime FechaModificacion { get; set; }

        [DisplayName("Creado Por")]
        public string CreadoPor { get; set; }

        [DisplayName("Modificado Por")]
        public string ModificadoPor { get; set; }
        [Display(Name = "Nombre de Contacto")]
        public string NombreContacto { get; set; }
        [Display(Name = "Teléfono de Contacto")]
        public string TelefonoContacto { get; set; }

        public string LogoName { get; set; } = null!;
        public string LogoPath { get; set; } = null!;

    }
}