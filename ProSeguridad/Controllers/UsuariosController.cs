using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProSeguridad.Claims;
using ProSeguridad.Data;
using ProSeguridad.Models;
using System.Security.Claims;
using static ProSeguridad.Models.ClaimsUsuarioWiewModel;

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
            var usuarios = _context.AppUsuarios.ToList();
            var roles= _context.Roles.ToList();
            var usuarioRol =_context.UserRoles.ToList();

            foreach (var usuario in usuarios)
            {
                var rol = usuarioRol.FirstOrDefault(u=>u.UserId==usuario.Id);
                if (rol==null)
                {
                    usuario.Rol = "Ninguno";
                }
                else
                {
                    usuario.Rol = roles.FirstOrDefault(u=>u.Id==rol.RoleId).Name;
                }
            }

            return View(usuarios);
        }
        [HttpGet]
        public IActionResult Editar(string id)
        {

            var usuarioDb = _context.AppUsuarios.FirstOrDefault(x => x.Id == id);
            if (usuarioDb ==null)
            {
                return NotFound();
            }

            var usuarioRol = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            var rol = usuarioRol.FirstOrDefault(x=>x.UserId==usuarioDb.Id);
            if (rol!=null)
            {
                usuarioDb.IdRol = roles.FirstOrDefault(x=>x.Id == rol.RoleId).Id;
            }
            usuarioDb.ListRol = _context.Roles.Select(x=> new SelectListItem()
            {
                Text=x.Name,
                Value=x.Id
            });

            return View(usuarioDb);
        
        }
        [HttpPost]
        public async Task<IActionResult> Editar(AppUsuario appUsuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioDb = _context.AppUsuarios.FirstOrDefault(x => x.Id == appUsuario.Id);
                if (usuarioDb == null)
                {
                    return NotFound();
                }
                var usuarioRol = _context.UserRoles.FirstOrDefault(x => x.UserId == usuarioDb.Id);

                if (usuarioRol != null)
                {
                    var RolActual = _context.Roles.Where(x => x.Id == usuarioRol.RoleId).Select(x => x.Name).FirstOrDefault();
                    await _userManager.RemoveFromRoleAsync(usuarioDb, RolActual);
                }

                await _userManager.AddToRoleAsync(usuarioDb, _context.Roles.FirstOrDefault(x => x.Id == appUsuario.IdRol).Name);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            appUsuario.ListRol = _context.Roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id
            });

            return View();

        }

        [HttpPost]
        public IActionResult BloqueoDesbloqueo(string IdUser)
        {
            var usuarioDb = _context.AppUsuarios.FirstOrDefault(x=>x.Id ==IdUser);
            if (usuarioDb==null)
            {
                return NotFound();
            }

            if (usuarioDb.LockoutEnd!=null&& usuarioDb.LockoutEnd>DateTime.Now)
            {
                usuarioDb.LockoutEnd = DateTime.Now;
                TempData["correcto"] = "Usuario bloqueado correctamente";
            }
            else
            {
                usuarioDb.LockoutEnd = DateTime.Now.AddYears(34);
                TempData["correcto"] = "";

            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Borrar(string IdUser)
        {
            var usuarioDb = _context.AppUsuarios.FirstOrDefault(x => x.Id == IdUser);
            if (usuarioDb == null)
            {
                return NotFound();
            }
            _context.AppUsuarios.Remove(usuarioDb);

            _context.SaveChanges();
            TempData["correcto"] = "";
            return RedirectToAction(nameof(Index));

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

       

        [HttpGet]
        public async Task<IActionResult> AdministrarClaimsUsuario(string IdUsuario)
        {
            IdentityUser usuario = await _userManager.FindByIdAsync(IdUsuario);
            if (usuario == null)
            {
                return NotFound();
            }

            var claimUsuarioActual = await _userManager.GetClaimsAsync(usuario);
            var modelo = new ClaimsUsuarioWiewModel()
            {
                IdUsuario = IdUsuario
            };

            foreach (Claim claim in ManejoClaims.ListaClaim)
            {
                ClaimUsuario claimUsuario = new ClaimUsuario()
                {
                    TipoClaim = claim.Type
                };

                if (claimUsuarioActual.Any(c => c.Type == claim.Type))
                {
                    claimUsuario.Seleccionando = true;
                }
                modelo.Claimss.Add(claimUsuario);
            }
            return View(modelo);
        }
        
        [HttpPost]
        public async Task<IActionResult> AdministrarClaimsUsuario(ClaimsUsuarioWiewModel wiewModelclaim)
        {
            IdentityUser usuario = await _userManager.FindByIdAsync(wiewModelclaim.IdUsuario);
            if (usuario == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(usuario);
           var resultado = await _userManager.RemoveClaimsAsync(usuario, claims);
           


         resultado = await _userManager.AddClaimsAsync(usuario, wiewModelclaim.Claimss.Where(c=> c.Seleccionando)
              .Select(cl=> new Claim(cl.TipoClaim, cl.Seleccionando.ToString()) ));
           
            if (!resultado.Succeeded)
            {
                return View(wiewModelclaim);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
