using Aduanas.Aci.Usuarios.Api.Services.Interfaces;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    /// <summary>
    /// Implementación temporal mientras no se integra un proveedor real de correo
    /// (SMTP, SendGrid, etc.). Por ahora solo registra el envío en el log.
    /// El link real se puede consultar habilitando "ReinicioContrasenia:DevolverLinkEnResponse"
    /// en appsettings.json (solo recomendado en Development/QA).
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task EnviarCorreoReinicioContraseniaAsync(string correoDestino, string nombreUsuario, string linkReinicio)
        {
            // TODO: reemplazar por la integración real del servicio de correo
            _logger.LogInformation(
                "[EmailService] Pendiente de envío real - Destino: {Correo} - Usuario: {Usuario} - Link: {Link}",
                correoDestino, nombreUsuario, linkReinicio);

            return Task.CompletedTask;
        }
    }
}
