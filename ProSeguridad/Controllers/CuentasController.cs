using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProSeguridad.Data;
using ProSeguridad.Models;

namespace ProSeguridad.Controllers
{
    public class CuentasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CuentasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager; 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(string? returnurl = null)
        {
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
    }


    

}
