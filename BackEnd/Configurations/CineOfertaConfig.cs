using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class CineOfertaConfig : IEntityTypeConfiguration<CineOferta>
    {
        public void Configure(EntityTypeBuilder<CineOferta> builder)
        {
            //Builder for CineOferta
            builder.Property(prop => prop.PorcentajeDescuento)
                    .HasPrecision(precision: 5, scale: 2);

            //builder.Property(prop => prop.FechaInicio)
            //        .HasColumnType("date");

            //builder.Property(prop => prop.FechaFin)
            //        .HasColumnType("date");
        }
    }
}
