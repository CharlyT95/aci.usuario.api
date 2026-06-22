using Aduanas.Aci.Usuarios.Api.DTOs.ReinicioContrasenia;

namespace Aduanas.Aci.Usuarios.Api.Services.Interfaces
{
    public interface IReinicioContraseniaService
    {
        /// <summary>
        /// Genera el token de reinicio y dispara el correo.
        /// Devuelve el link SOLO si "DevolverLinkEnResponse" está habilitado en configuración appsettings.json
        /// </summary>
        Task<string?> SolicitarReinicio(SolicitarReinicioDTO dto);

        Task<bool> ConfirmarReinicio(ConfirmarReinicioDTO dto);
    }
}
