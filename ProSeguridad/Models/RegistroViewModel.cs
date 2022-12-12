using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProSeguridad.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage ="El email es obligatorio")]      
        public string? Email { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        [StringLength(50, ErrorMessage ="{0} de estar entre al menos {2} caracteres de longitud", MinimumLength =5)]
        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "El confirmacion de la contraseña es obligatorio")]
        [Compare("Password", ErrorMessage ="La contrseña y confirmacion de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confimar Contraseña")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage ="El nombre es obligatorio")]
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public int? CodigoPais { get; set; }
        public string? Telefono { get; set; }
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool? Estado { get; set; }

        [Display(Name ="Seleccionar rol")]
        public IEnumerable<SelectListItem>? ListaRoles { get; set; }
        public string? RolSeleccionado { get; set; }


    }
}
