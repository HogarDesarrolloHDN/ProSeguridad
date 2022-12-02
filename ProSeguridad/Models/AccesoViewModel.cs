using System.ComponentModel.DataAnnotations;

namespace ProSeguridad.Models
{
    public class AccesoViewModel
    {
        [Required(ErrorMessage ="El email es obligatorio")]      
        public string? Email { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        public string? Password { get; set; }

        [Display(Name ="Recodar datos?")]
        public bool RememberMe { get; set; }

    }
}
