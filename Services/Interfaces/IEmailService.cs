namespace Aduanas.Aci.Usuarios.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarCorreoReinicioContraseniaAsync(string correoDestino, string nombreUsuario, string linkReinicio);
    }
}
