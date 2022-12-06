using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProSeguridad.Models
{
    public class RecuperarPasswordViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "El confirmacion de la contraseña es obligatorio")]
        [Compare("Password", ErrorMessage = "La contrseña y confirmacion de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confimar Contraseña")]
        public string? ConfirmPassword { get; set; }
        public string? Code { get; set; }
    }
}
