using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.ReinicioContrasenia
{
    public class SolicitarReinicioDTO
    {
        [Required(ErrorMessage = "Debe ingresar el usuario o correo electrónico")]
        public string UsuarioCorreo { get; set; }
    }
}
