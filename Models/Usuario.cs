using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ExpediFlow.Models
{
    public class Usuario : IdentityUser
    {
        //[DisplayName("Nombre de Usuario")]
        //public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Fotografía")]

        public byte[] Avatar { get; set; } = null!;
        public string AvatarName { get; set; } = null!;
        public string AvatarPath { get; set; } = null!;

        [NotMapped]
        public string AvatarBase64 { get; set; } = null!;


    }
}
