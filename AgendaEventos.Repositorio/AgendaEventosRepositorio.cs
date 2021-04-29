using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaEventos.Dominio;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace AgendaEventos.Repositorio
{
    public class AgendaEventosRepositorio : IAgendaEventosRepositorio
    {
        private readonly AgendaEventosContext _context;
        private readonly string _conectionString;

        public AgendaEventosRepositorio(AgendaEventosContext context, IConfiguration configuration)
        {
            _context = context;
            _conectionString = configuration.GetConnectionString("AgendaEventos");
        }
        //Geral
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
        return (await _context.SaveChangesAsync()) > 0;
        }

        //Eventos
        public IEnumerable<Evento> GetQtdEventoTipoExclusivo()
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {

                var listaEventos = connection.Query<Evento>("select  count(Tipo) from Eventos group by Tipo having Tipo = 'E'");
                return listaEventos;
            }
        }
        public List<Evento> GetAllEventosAsync()
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {

                var listaEventos = connection.Query<Evento>("select * from Eventos");
                return listaEventos.ToList();
            }
        }

        public List<Evento> GetAllEventosByDataAsync(string dataInicio, string dataFim)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var listaEventos = connection.Query<Evento>("select * from Eventos where DataEvento between @dataInicio and @dataFim", new { @dataInicio = dataInicio, @dataFim = dataFim} );
                return listaEventos.ToList();
            }
        }

        public List<Evento> GetAllEventosByTemaAsync(string tema)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var listaEventos = connection.Query<Evento>("select * from Eventos where Tema like '%'+@tema+'%'", new { @tema = tema } );
                return listaEventos.ToList();
            }
        }
    

        public List<Evento> GetAllEventoUsuarioAsync(int usuarioId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var sql =@"select E.Id, E.DataEvento, E.Tema, E.Descricao, E.Tipo, U.Nome
                            from Eventos E
                            inner Join UsuariosEventos UE on E.Id = UE.EventoId inner Join Usuarios U on UE.UsuarioId = U.Id
                            where UE.UsuarioId = @usuarioId    
                        ";
                var evento = connection.Query<Evento, Usuario, Evento>(sql, MapResults, splitOn: "Nome");

                return evento.ToList();
            }
        }
        private Evento MapResults (Evento evento, Usuario user){
                    evento.Usuario = user;
                    return evento;
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT *      
                            From Eventos
                            where Id = @Id
                        ";

                var evento = connection.QueryMultiple(sql, new { @Id = eventoId });
                var result = evento.ReadSingle<Evento>();
                return result;  
            }
        }

        //Usuario
        public async Task<Usuario> GetUsuarioByIdAsync(int usuarioId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT *      
                            From Usuarios
                            where Id = @Id
                        ";

                var usuario = connection.QueryMultiple(sql, new { @Id = usuarioId });
                var result = usuario.ReadSingle<Usuario>();
                return result;  
            }
        }

        public async Task<Usuario> GetUsuarioByNomeAsync(string nomeUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT Nome      
                            From Usuarios
                            where Nome = @nome
                        ";

                var usuario = connection.QueryMultiple(sql, new { @nome = nomeUsuario });
                var result = usuario.ReadSingle<Usuario>();
                return result;  
            }
        }

        public async Task<Usuario> GetUsuarioBySenhaAsync(string nomeUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT *      
                            From Usuarios
                            where nome = @nome
                        ";

                var usuario = connection.QueryMultiple(sql, new { @nome = nomeUsuario });
                var result = usuario.ReadSingle<Usuario>();
                return result;  
            }
        }
    }
}