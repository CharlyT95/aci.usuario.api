using Aduanas.Aci.Usuarios.Api.DTOs.ReinicioContrasenia;
using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Helpers;

namespace Aduanas.Aci.Usuarios.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    [AllowAnonymous] // El usuario aún no tiene sesión cuando olvida su contraseña
    public class ReinicioContraseniaController : ControllerBase
    {
        private readonly IReinicioContraseniaService _reinicioContraseniaService;

        public ReinicioContraseniaController(IReinicioContraseniaService reinicioContraseniaService)
        {
            _reinicioContraseniaService = reinicioContraseniaService;
        }

        [HttpPost("SolicitarReinicioContrasenia")]
        public async Task<IActionResult> SolicitarReinicio([FromBody] SolicitarReinicioDTO dto)
        {
            var link = await _reinicioContraseniaService.SolicitarReinicio(dto);

            const string mensaje = "Se enviará un enlace para restablecer la contraseña.";

            // "link" solo viene distinto de null si ReinicioContrasenia:DevolverLinkEnResponse
            // está activo en appsettings (uso temporal mientras no exista el servicio de correo)
            object? datos = link != null ? new { LinkReinicio = link } : null;

            return Ok(ResponseHelper.Success(datos, mensaje));
        }

        [HttpPost("ConfirmarReinicioContrasenia")]
        public async Task<IActionResult> ConfirmarReinicio([FromBody] ConfirmarReinicioDTO dto)
        {
            var data = await _reinicioContraseniaService.ConfirmarReinicio(dto);
            return Ok(ResponseHelper.Success(data));
        }
    }
}
