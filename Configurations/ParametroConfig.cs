using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class ParametroConfig : IEntityTypeConfiguration<Parametro>
    {
        public void Configure(EntityTypeBuilder<Parametro> builder)
        {
            builder.ToTable("Parametro");

            builder.HasKey(x => x.IdParametro);

            builder.Property(x => x.CodigoParametro).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Valor).HasMaxLength(500).IsRequired();
            builder.Property(x => x.UsuarioCreacion).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UsuarioModificacion).HasMaxLength(100);

            builder.HasIndex(x => x.CodigoParametro).IsUnique();
        }
    }
}
