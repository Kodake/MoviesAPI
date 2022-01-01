using BackEnd.Configurations;
using BackEnd.Entities;
using BackEnd.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackEnd
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //This was added because is a better way to add all the dependencies without all the code below:
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedingModuloConsulta.Seed(modelBuilder);

            //Intentionally commented just for learning:
            //modelBuilder.ApplyConfiguration(new ActorConfig());
            //modelBuilder.ApplyConfiguration(new CineConfig());
            //modelBuilder.ApplyConfiguration(new CineOfertaConfig());
            //modelBuilder.ApplyConfiguration(new GeneroConfig());
            //modelBuilder.ApplyConfiguration(new PeliculaActorConfig());
            //modelBuilder.ApplyConfiguration(new PeliculaConfig());
            //modelBuilder.ApplyConfiguration(new SalaDeCineConfig());
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<CineOferta> CinesOfertas { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
    }
}
