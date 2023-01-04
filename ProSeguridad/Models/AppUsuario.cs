using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProSeguridad.Models
{
    public class AppUsuario: IdentityUser
    {
        public  string? Nombre { get; set; }
        public string? Url { get; set; }
        public int? CodigoPais { get; set; }
        public string? Telefono { get; set; }
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool?  Estado { get; set; }

        [NotMapped]
        public string? Rol { get ; set; }
        [NotMapped]
        [Display(Name ="Lista de rol")]
        public string? IdRol { get; set; }
        [NotMapped] 
        public IEnumerable<SelectListItem>? ListRol { get; set; }    
    }
}
