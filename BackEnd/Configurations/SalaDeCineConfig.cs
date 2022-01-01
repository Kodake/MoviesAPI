using BackEnd.Entities;
using BackEnd.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class SalaDeCineConfig : IEntityTypeConfiguration<SalaDeCine>
    {
        public void Configure(EntityTypeBuilder<SalaDeCine> builder)
        {
            //Builder for SalaDeCine
            builder.Property(prop => prop.Precio)
                    .HasPrecision(9, 2);

            builder.Property(prop => prop.TipoSalaDeCine)
                    .HasDefaultValue(TipoSalaDeCine.DosDimensiones);
        }
    }
}
