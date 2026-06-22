using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class ParametroService : IParametroService
    {
        private readonly UserManagementDbContext _context;

        public ParametroService(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ObtenerValorAsync(string codigoParametro)
        {
            var parametro = await _context.Parametro
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.CodigoParametro == codigoParametro && p.Activo);

            return parametro?.Valor;
        }

        public async Task<int> ObtenerValorIntAsync(string codigoParametro, int valorPorDefecto)
        {
            var valor = await ObtenerValorAsync(codigoParametro);

            if (valor != null && int.TryParse(valor, out var resultado))
                return resultado;

            return valorPorDefecto;
        }
    }
}
