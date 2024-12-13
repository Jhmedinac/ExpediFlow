using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpediFlow.Data.Migrations;
using Microsoft.AspNetCore.Identity;

namespace ExpediFlow.ViewModel
{
    public class ProfileViewModel : Usuario
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string PhoneNumber { get; set; }

        [DisplayName("Fotografía")]
        public byte[] Avatar { get; set; } = null!;


        public string AvatarName { get; set; }

        public string AvatarPath { get; set; }

        [NotMapped]
        public string AvatarBase64 { get; set; } = null!;
    }
}
