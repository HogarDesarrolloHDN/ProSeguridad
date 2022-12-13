using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProSeguridad.Data;
using ProSeguridad.Models;

namespace ProSeguridad.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsuariosController(UserManager<IdentityUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditarPerfil( string id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var usuarioDb = _context.AppUsuarios.Find(id);
            if (usuarioDb == null)
            {
                return NotFound();
            }
            return View(usuarioDb);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(AppUsuario appUsuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioDb = _context.AppUsuarios.Find(appUsuario.Id);

                usuarioDb.Nombre=appUsuario.Nombre;
                usuarioDb.Url=appUsuario.Url;
                usuarioDb.CodigoPais=appUsuario.CodigoPais;
                usuarioDb.Telefono= appUsuario.Telefono;
                usuarioDb .Ciudad =appUsuario.Ciudad;
                usuarioDb.Pais=appUsuario.Pais;
                usuarioDb.Direccion=appUsuario.Direccion;
                usuarioDb.FechaNacimiento=appUsuario.FechaNacimiento;


                await _userManager.UpdateAsync(usuarioDb);
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(appUsuario);       
        
        }

        [HttpGet]
        public IActionResult CambiarPassword()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel cambiarPassword, string email)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                if (usuario==null)
                {
                    return RedirectToAction("Error");
                }

                var token =await _userManager.GeneratePasswordResetTokenAsync(usuario);

                var resultado = await _userManager.ResetPasswordAsync(usuario, token, cambiarPassword.Password );

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionCambioPassword");
                }
                else
                {
                    return View(cambiarPassword);
                }
            }
            return View(cambiarPassword);

        }

           [HttpGet]
        public IActionResult ConfirmacionCambioPassword()
        {
            return View();
        }

    }
}
