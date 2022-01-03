using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class GeneroConfig : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            //Builder for Genero
            builder.HasKey(prop => prop.Identificador);

            builder.Property(prop => prop.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.HasQueryFilter(prop => !prop.EstaBorrado);
        }
    }
}
