using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class PeliculaConfig : IEntityTypeConfiguration<Pelicula>
    {
        public void Configure(EntityTypeBuilder<Pelicula> builder)
        {
            //Builder for Pelicula
            builder.Property(prop => prop.Titulo)
                    .HasMaxLength(100)
                    .IsRequired();

            //builder.Property(prop => prop.FechaEstreno)
            //      .HasColumnType("date");

            builder.Property(prop => prop.PosterURL)
                    .HasMaxLength(500)
                    .IsUnicode(false);
        }
    }
}
