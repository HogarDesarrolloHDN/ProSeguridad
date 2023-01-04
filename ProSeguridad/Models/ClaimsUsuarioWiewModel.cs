namespace ProSeguridad.Models
{
    public class ClaimsUsuarioWiewModel
    {
       
        public ClaimsUsuarioWiewModel()
        {
            Claimss = new List<ClaimUsuario>();
        }


        public string IdUsuario { get; set; }
        public  List<ClaimUsuario> Claimss { get;set; }

        public class ClaimUsuario
        {
            public string TipoClaim { get; set; }
            public bool Seleccionando { get; set; }    
        }
    }
}
