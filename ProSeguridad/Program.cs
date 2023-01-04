using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProSeguridad.Data;
using ProSeguridad.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//cadena de conecxion
builder.Services.AddDbContext<ApplicationDbContext>(option =>
 option.UseSqlServer(builder.Configuration.GetConnectionString("conections")));

//Agregar servicio de identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",policy=>policy.RequireRole("Admin"));
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
    options.AddPolicy("Registrado", policy => policy.RequireRole("Registrado"));
    options.AddPolicy("AccesoUsuarioYAdministrador", policy => policy.RequireRole("Usuario").RequireRole("Admin"));
   
    options.AddPolicy("AdministradorPermisoCrear", policy => policy.RequireRole("Admin").RequireClaim("Crear", "True"));
    options.AddPolicy("AdministradorPermisoEditar", policy => policy.RequireRole("Admin").RequireClaim("Editar", "True"));
    options.AddPolicy("AdministradorPermisoBorrar", policy => policy.RequireRole("Admin").RequireClaim("Borrar", "True"));

    options.AddPolicy("AdministradorPermisoCrearEditarBorrar", policy => policy.RequireRole("Admin").RequireClaim("Crear", "True")
    .RequireClaim("Editar", "True").RequireClaim("Borrar", "True"));



});

///Se Agrega EmailSender
builder.Services.AddTransient<IEmailSender, MaiJetEmailSender>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
