using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class ReinicioContraseniaConfig : IEntityTypeConfiguration<ReinicioContrasenia>
    {
        public void Configure(EntityTypeBuilder<ReinicioContrasenia> builder)
        {
            builder.ToTable("ReinicioContrasenia");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.FechaCreacion).IsRequired();
            builder.Property(x => x.FechaExpiracion).IsRequired();
            builder.Property(x => x.Usado).HasDefaultValue(false);

            builder.HasIndex(x => x.IdUsuario);

            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.IdUsuario);
        }
    }
}
