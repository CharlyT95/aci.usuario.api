using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.ReinicioContrasenia
{
    public class ConfirmarReinicioDTO
    {
        [Required(ErrorMessage = "El token es requerido")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Debe ingresar la nueva contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string PasswordNueva { get; set; }
    }
}
