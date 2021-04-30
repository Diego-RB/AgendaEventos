using AgendaEventos.Dominio.Identity;

namespace AgendaEventos.Dominio
{
    public class UsuarioEvento
    {
        public int UsuarioId { get; set; }
        public User User { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}