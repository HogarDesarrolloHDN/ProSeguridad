using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProSeguridad.Controllers
{
    public class AutorizacionController : Controller
    {
        [AllowAnonymous]
        public IActionResult AccesoPublico()
        {
            return View();
        }
        [Authorize]
        public IActionResult AccesoAutenticado()
        {
            return View();
        }

        [Authorize(Policy = "Usuario")]
        public IActionResult AccesoUsuario()
        {
            return View();
        }

        [Authorize(Policy = "Registrado")]
        public IActionResult AccesoRegistrado()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public IActionResult AccesoAdministrador()
        {
            return View();
        }
        [Authorize(Roles  = "Usuario, Admin")]

        public IActionResult AccesoUsuarioAdministrador()
        {
            return View();
        }
        [Authorize(Policy = "AccesoUsuarioYAdministrador")]

        public IActionResult AccesoUsuarioYAdministrador()
        {
            return View();
        }

        [Authorize(Policy = "AdministradorPermisoCrear")]
        public IActionResult AccesoAdministradorPermisoCrear()
         {
            return View();
        }

        [Authorize(Policy = "AdministradorPermisoEditar")]
        public IActionResult AccesoAdministradorPermisoEditar()
        {
            return View();
        }


        [Authorize(Policy = "AdministradorPermisoBorrar")]
        public IActionResult AccesoAdministradorPermisoBorrar()
        {
            return View();
        }


        [Authorize(Policy = "AdministradorPermisoCrearEditarBorrar")]
        public IActionResult AccesoAdministradorPermisoCrearEditarBorrar()
        {
            return View();
        }
    }
}
