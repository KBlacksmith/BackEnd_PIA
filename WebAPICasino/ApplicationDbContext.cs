using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPICasino.Entidades;

namespace WebAPICasino{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }
        protected override void OnModelCreating (ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ParticipanteRifa>().HasKey(
                pr => new {pr.ParticipanteId, pr.RifaId}
            );
        }
        public DbSet<Rifa> Rifas {get; set;}
        public DbSet<Participante> Participantes {get; set; }
        public DbSet<BoletoDeLoteria> Boletos {get; set; }
        public DbSet<ParticipanteRifa> ParticipantesRifas {get; set; }
        public DbSet<Premio> Premios {get; set; }
    }
}