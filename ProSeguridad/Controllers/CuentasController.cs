using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProSeguridad.Data;
using ProSeguridad.Models;

namespace ProSeguridad.Controllers
{
    public class CuentasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public CuentasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager; 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(string? returnurl = null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("usuario"))
            {
                await _roleManager.CreateAsync(new IdentityRole("usuario"));
            }



            RegistroViewModel registroViewModel = new RegistroViewModel();

            return View(registroViewModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel registroViewModel, string? returnurl = null)
        {
            ViewData["ReturnurlUrl"] = returnurl;
            returnurl = returnurl??Url.Content("~/");

            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario()
                {
                    UserName = registroViewModel.Email,
                    Email = registroViewModel.Email,
                    Nombre = registroViewModel.Nombre,
                    Url = registroViewModel.Url,
                    CodigoPais = registroViewModel.CodigoPais,
                    Telefono = registroViewModel.Telefono,
                    Pais=registroViewModel.Pais,
                    Ciudad = registroViewModel.Ciudad,
                    Direccion = registroViewModel.Direccion,
                    FechaNacimiento = registroViewModel.FechaNacimiento,
                    Estado=registroViewModel.Estado,                  

                };
               var resultado = await _userManager.CreateAsync(usuario, registroViewModel.Password);
               
                if (resultado.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario,"Admin");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                    var urlReturn = Url.Action("ConfirmarEmail", "Cuentas", new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);
                   
                    await _emailSender.SendEmailAsync(registroViewModel.Email, "Confirmacion de cuenta - proyecto seguridad",
                        "Por favor confirme su cuenta dando click aquí: <a href=\"" + urlReturn + "\">enlace</a>");

                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                   return LocalRedirect(returnurl);
                   
                }
                ValidarErrores(resultado);
            }

            return View(registroViewModel);
        }


        //Manejador de errores
        private void ValidarErrores(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RegistroAdmin(string? returnurl = null)
        {

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("usuario"))
            {
                await _roleManager.CreateAsync(new IdentityRole("usuario"));
            }

            List<SelectListItem> listaRoles = new List<SelectListItem>();

            listaRoles.Add(new SelectListItem()
            {
                Value= "usuario",
                Text= "usuario"
            });

            listaRoles.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });

            RegistroViewModel registroViewModel = new RegistroViewModel()
            {
                ListaRoles = listaRoles,
            };

            return View(registroViewModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RegistroAdmin(RegistroViewModel registroViewModel, string? returnurl = null)
        {
            ViewData["ReturnurlUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario()
                {
                    UserName = registroViewModel.Email,
                    Email = registroViewModel.Email,
                    Nombre = registroViewModel.Nombre,
                    Url = registroViewModel.Url,
                    CodigoPais = registroViewModel.CodigoPais,
                    Telefono = registroViewModel.Telefono,
                    Pais = registroViewModel.Pais,
                    Ciudad = registroViewModel.Ciudad,
                    Direccion = registroViewModel.Direccion,
                    FechaNacimiento = registroViewModel.FechaNacimiento,
                    Estado = registroViewModel.Estado,

                };
                var resultado = await _userManager.CreateAsync(usuario, registroViewModel.Password);

                if (resultado.Succeeded)
                {
                    if (registroViewModel.RolSeleccionado!=null && registroViewModel.RolSeleccionado.Length>=0 && registroViewModel.RolSeleccionado=="Admin")
                    {
                        await _userManager.AddToRoleAsync(usuario, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(usuario, "usuario");

                    }
                            

                   

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                    var urlReturn = Url.Action("ConfirmarEmail", "Cuentas", new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(registroViewModel.Email, "Confirmacion de cuenta - proyecto seguridad",
                        "Por favor confirme su cuenta dando click aquí: <a href=\"" + urlReturn + "\">enlace</a>");

                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return LocalRedirect(returnurl);

                }
                ValidarErrores(resultado);
            }
            List<SelectListItem> listaRoles = new List<SelectListItem>();

            listaRoles.Add(new SelectListItem()
            {
                Value = "usuario",
                Text = "usuario"
            });

            listaRoles.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });

            registroViewModel.ListaRoles = listaRoles;  

            return View(registroViewModel);
        }

      

        [HttpGet]
        public IActionResult Acceso( string? returnurl=null)
        {
            ViewData["ReturnurlUrl"] = returnurl;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Acceso(AccesoViewModel accesoView, string? returnurl=null)
        {
            ViewData["ReturnurlUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {           
                var resultado = await _signInManager.PasswordSignInAsync(accesoView.Email, accesoView.Password , accesoView.RememberMe, lockoutOnFailure:true);

                if (resultado.Succeeded)
                {                    
                    return LocalRedirect(returnurl);
                }
                if (resultado.IsLockedOut)
                {
                    return View("Bloqueado");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acceso invalido");
                    return View(accesoView);
                }
                
            }
            return View(accesoView);
        }

        //salirO cerrar sesion
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> SalirAplicacion() {        
             await _signInManager.SignOutAsync();
           return RedirectToAction(nameof(HomeController.Index),"Home");
        
        }

        //Metodo para olvido password
        [HttpGet]
        public async Task<IActionResult> OlvidoPassword()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel olvidoPassword)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(olvidoPassword.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionOlvidoPassword");
                }

                var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var urlRetorno = Url.Action("ResetPassword", "Cuentas", new {userId=usuario.Id, code = codigo},protocol:HttpContext.Request.Scheme);
               
                await _emailSender.SendEmailAsync(olvidoPassword.Email, "Recuperar contraseña - Proyecto Identity",
                    "Por favor recupere su contraseña dando click aquí: <a href=\""+urlRetorno+"\">enlace</a>");
                return RedirectToAction("ConfirmacionOlvidoPassword");
            }
            return View(olvidoPassword);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionOlvidoPassword()
        {
            return View();
        }

        [HttpGet]        
        public IActionResult ResetPassword( string? code = null)
        {
            return code == null? View("Error"):View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(RecuperarPasswordViewModel recuperar)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(recuperar.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }
                var resultado= await _userManager.ResetPasswordAsync(usuario, recuperar.Code, recuperar.Password);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }
               ValidarErrores(resultado);
            }
            return View(recuperar);
        }
        

        [HttpGet]
        public IActionResult ConfirmacionRecuperaPassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Bloqueado(string? returnurl = null)
        {
            ViewData["ReturnurlUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(string userId, string code)
         {
            if (userId==null || code==null)
            {
                return View("Error");
            }

          var usuario =  await _userManager.FindByIdAsync(userId);
            if (usuario==null)
            {
                return View("Error");
            }
            var resultado=await _userManager.ConfirmEmailAsync(usuario, code);
            if (resultado.Succeeded)
            {
                return View("ConfirmarEmail");
            }
            else
            {
                return View("Error");
            }

            
        }
    }
}


    


