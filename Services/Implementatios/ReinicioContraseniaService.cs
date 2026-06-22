using Aduanas.Aci.Usuarios.Api.DTOs.ReinicioContrasenia;
using Aduanas.Aci.Usuarios.Api.Errors.ReinicioContrasenia;
using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagementAPI.Data;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class ReinicioContraseniaService : IReinicioContraseniaService
    {
        private readonly UserManagementDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IParametroService _parametroService;

        private const string CodigoParametroMinutosExpiracion = "MinExpReinicioContrasenia";
        private const int MinutosExpiracionPorDefecto = 15;

        public ReinicioContraseniaService(
            UserManagementDbContext context,
            IPasswordService passwordService,
            IEmailService emailService,
            IConfiguration configuration,
            IParametroService parametroService)
        {
            _context = context;
            _passwordService = passwordService;
            _emailService = emailService;
            _configuration = configuration;
            _parametroService = parametroService;
        }

        public async Task<string?> SolicitarReinicio(SolicitarReinicioDTO dto)
        {
            var valor = dto.UsuarioCorreo?.Trim();

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u =>
                u.Activo &&
                (u.CorreoElectronico == valor || u.UsuarioLogin == valor));

            // No revelamos si el usuario existe
            if (usuario == null)
                return null;

            var tieneCredenciales = await _context.UsuarioCredencial
                .AnyAsync(c => c.IdUsuario == usuario.IdUsuario);

            if (!tieneCredenciales)
                return null;

            // Invalidar cualquier solicitud anterior que siga activa
            var pendientes = await _context.ReinicioContrasenia
                .Where(r => r.IdUsuario == usuario.IdUsuario && !r.Usado)
                .ToListAsync();

            foreach (var p in pendientes)
                p.Usado = true;

            // Token aleatorio (32 bytes) en formato seguro para URL
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var tokenHash = CalcularHash(token);

            var minutosExpiracion = await _parametroService.ObtenerValorIntAsync(
                CodigoParametroMinutosExpiracion, MinutosExpiracionPorDefecto);

            var registro = new UserManagementAPI.Models.ReinicioContrasenia
            {
                IdUsuario = usuario.IdUsuario,
                TokenHash = tokenHash,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(minutosExpiracion),
                Usado = false
            };

            _context.ReinicioContrasenia.Add(registro);
            await _context.SaveChangesAsync();

            var urlBase = _configuration["ReinicioContrasenia:UrlFrontend"];
            var link = $"{urlBase}?idenHash={token}";

            await _emailService.EnviarCorreoReinicioContraseniaAsync(
                usuario.CorreoElectronico, usuario.Nombres, link);

            var devolverLink = _configuration.GetValue<bool>("ReinicioContrasenia:DevolverLinkEnResponse", false);
            return devolverLink ? link : null;
        }

        public async Task<bool> ConfirmarReinicio(ConfirmarReinicioDTO dto)
        {
            var tokenHash = CalcularHash(dto.Token);

            var registro = await _context.ReinicioContrasenia.FirstOrDefaultAsync(r =>
                r.TokenHash == tokenHash &&
                !r.Usado &&
                r.FechaExpiracion > DateTime.Now);

            if (registro == null)
                throw new Exception(ReinicioContraseniaErrors.TokenInvalido);

            var credencial = await _context.UsuarioCredencial
                .FirstOrDefaultAsync(c => c.IdUsuario == registro.IdUsuario);

            if (credencial == null)
                throw new Exception(ReinicioContraseniaErrors.UsuarioSinCredenciales);

            _passwordService.CreatePassword(dto.PasswordNueva, out byte[] hash, out byte[] salt);

            credencial.PasswordHash = hash;
            credencial.PasswordSalt = salt;
            credencial.FechaUltimoCambio = DateTime.Now;
            credencial.IntentosFallidos = 0;
            credencial.BloqueoTemporal = false;

            registro.Usado = true;

            await _context.SaveChangesAsync();

            return true;
        }

        private static string CalcularHash(string valor)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(valor));
            return Convert.ToBase64String(bytes);
        }
    }
}
