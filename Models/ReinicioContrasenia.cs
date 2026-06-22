namespace UserManagementAPI.Models
{
    public class ReinicioContrasenia
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        // Hash del token (nunca se guarda el token en texto plano)
        public string TokenHash { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }
    }
}
