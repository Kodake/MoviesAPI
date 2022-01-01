using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations
{
    public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
    {
        public void Configure(EntityTypeBuilder<PeliculaActor> builder)
        {
            //Builder for PeliculaActor
            builder.HasKey(prop =>
            new { prop.PeliculaId, prop.ActorId });

            builder.Property(prop => prop.Personaje)
                    .HasMaxLength(100);
        }
    }
}
