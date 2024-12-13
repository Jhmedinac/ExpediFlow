using System.ComponentModel.DataAnnotations;

namespace ExpediFlow.ViewModel
{
    public class EmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Nuevo Correo Electrónico")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
