using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProSeguridad.Data;

namespace ProSeguridad.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(IdentityRole role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                TempData["Error"] = "El rol exixte no puedes creal";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.CreateAsync( new IdentityRole() { Name=role.Name});
            TempData["correcto"] = "rol creado correctamente";
            return RedirectToAction(nameof(Index));
            

        }
    
        [HttpGet]
        public IActionResult Editar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var roll = _context.Roles.FirstOrDefault(x=>x.Id==id);
                return View(roll);
            }
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(IdentityRole role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                TempData["Error"] = "El rol ya exixte";
                return RedirectToAction(nameof(Index));
            }
            var rolDb = _context.Roles.FirstOrDefault(x => x.Id == role.Id);
            if (rolDb == null)
            {
                return RedirectToAction(nameof(Index));
            }
            rolDb.Name = role.Name;
            rolDb.NormalizedName = role.Name.ToUpper();
            await _roleManager.UpdateAsync(rolDb);
            TempData["correcto"] = "Rol editado correctamente";
            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(string id)
        {
            var rolDb = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (rolDb == null)
            {
                TempData["Error"] = "No existe el rol";
                return RedirectToAction(nameof(Index));
            }
            var usuariorol = _context.UserRoles.Where(u=>u.RoleId==id).Count();
            if (usuariorol>0)
            {
                TempData["Error"] = "El Rol tiene usuario no se puede borrar";
                return RedirectToAction(nameof(Index));
            }           
        
            await _roleManager.DeleteAsync(rolDb);
            TempData["correcto"] = "Rol borrado correctamente";

            return RedirectToAction(nameof(Index));


        }

    }
}
