using System.Security.Claims;

namespace ProSeguridad.Claims
{
    public static class ManejoClaims
    {

        public static List<Claim> ListaClaim = new List<Claim>()
        {
            new Claim("Crear","Crear"),
            new Claim("Editar","Editar"),
            new Claim("Borrar","Borrar")
        };

    }
}
