using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProSeguridad.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//cadena de conecxion
builder.Services.AddDbContext<ApplicationDbContext>(option =>
 option.UseSqlServer(builder.Configuration.GetConnectionString("conections")));

//Agregar servicio de identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
/// reinscribe mi url paraacceso

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = new PathString("/Cuentas/Acceso");
    options.AccessDeniedPath = new PathString("/Cuentas/Bloqueado");
});

//esta linea para la url de retorno
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 5;
    option.Password.RequireLowercase = true;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    option.Lockout.MaxFailedAccessAttempts = 3;

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
