using AgendaEventos.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AgendaEventos.Repositorio
{
    public class AgendaEventosContext : DbContext
    {
        public AgendaEventosContext(DbContextOptions<AgendaEventosContext> options) : base(options) { }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioEvento> UsuariosEventos { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioEvento>()
                .HasKey(UE => new {UE.EventoId, UE.UsuarioId});

        }
    }
}