using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class CineConfig : IEntityTypeConfiguration<Cine>
    {
        public void Configure(EntityTypeBuilder<Cine> builder)
        {
            //Builder for Cine
            builder.Property(prop => prop.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();
        }
    }
}
