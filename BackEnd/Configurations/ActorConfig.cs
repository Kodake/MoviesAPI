using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            //Builder for Actor
            builder.Property(prop => prop.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();

            //builder.Property(prop => prop.FechaNacimiento)
            //      .HasColumnType("date");
        }
    }
}
