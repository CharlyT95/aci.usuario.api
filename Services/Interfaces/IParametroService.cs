namespace Aduanas.Aci.Usuarios.Api.Services.Interfaces
{
    public interface IParametroService
    {
        /// <summary>
        /// Obtiene el valor (texto) de un parámetro activo. Null si no existe.
        /// </summary>
        Task<string?> ObtenerValorAsync(string codigoParametro);

        /// <summary>
        /// Obtiene el valor de un parámetro convertido a int.
        /// Si no existe, está inactivo, o no es convertible, devuelve valorPorDefecto.
        /// </summary>
        Task<int> ObtenerValorIntAsync(string codigoParametro, int valorPorDefecto);
    }
}
