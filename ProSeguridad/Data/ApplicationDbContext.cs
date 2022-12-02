using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProSeguridad.Models;

namespace ProSeguridad.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext( DbContextOptions options):base(options)
        {

        }
        public DbSet<AppUsuario> AppUsuarios { get; set; }  
    }
}
