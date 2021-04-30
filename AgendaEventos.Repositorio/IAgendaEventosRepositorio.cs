using System.Collections.Generic;
using System.Threading.Tasks;
using AgendaEventos.Dominio;
using AgendaEventos.Dominio.Identity;

namespace AgendaEventos.Repositorio
{
    public interface IAgendaEventosRepositorio
    {
        //Geral
        void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         void DeleteRange<T>(T[] entityArray) where T: class;
         Task<bool> SaveChangesAsync();

         //Eventos
        List<Evento> GetAllEventosByTemaAsync(string tema);
        List<Evento> GetAllEventosByDataAsync(string dataInicio, string dataFim);
        List<Evento> GetAllEventosAsync();
        IEnumerable<Evento> GetQtdEventoTipoExclusivo();
        //List<Evento> GetAllEventoUsuarioAsync(int usuarioId);
        Task<Evento> GetEventoByIdAsync(int eventoId);

        //Usuario
        Task<User> GetUsuarioByIdAsync(int usuarioId);
        Task<User> GetUsuarioByNomeAsync(string nomeUsuario);
        Task<User> GetUsuarioBySenhaAsync(string nomeUsuario);
    }
}