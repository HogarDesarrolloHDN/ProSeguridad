using System.ComponentModel.DataAnnotations;

namespace ProSeguridad.Models
{
    public class OlvidoPasswordViewModel
    {
        [Required(ErrorMessage ="El email es obligatorio")]      
        public string? Email { get; set; }

       

    }
}
