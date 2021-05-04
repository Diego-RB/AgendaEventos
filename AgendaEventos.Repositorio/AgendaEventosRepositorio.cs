using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaEventos.Dominio;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using AgendaEventos.Dominio.Identity;
using System.Globalization;
using System.Threading;
using System;

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

        public async Task<int> GetQtdUsuariosEvento(int eventoId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var sql =@"select COUNT(UE.EventoId) as QtdParticipantes
                            from Eventos E
                            inner Join UsuariosEventos UE on E.Id = UE.EventoId inner Join AspNetUsers U on UE.UsuarioId = U.Id
                            group by UE.EventoId
                            having UE.EventoId = @eventoId";
                var evento = connection.QueryMultiple(sql, new { @eventoId = eventoId });
                var result = evento.ReadSingle<int>();
                return result;
            }
        }
         public List<Evento> GetAllEventos()
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var sql =@"SELECT  E.Id ,E.Local ,E.DataEvento, E.Tipo ,E.Tema ,E.Descricao ,U.UserName
                            FROM Eventos E
                            inner Join AspNetUsers U on E.UserId = U.Id
                            Order by DataEvento asc  
                        ";
                var evento = connection.Query<Evento, User, Evento>(sql, Results, splitOn: "UserName");

                return evento.ToList();
            }
        }

        private Evento Results (Evento evento, User user){
            evento.User = user;
            return evento;
        }

        public async Task<List<Evento>> GetUsuarioParticipandoEvento(int eventoId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var sql =@"select E.Id, E.DataEvento, E.Tema, E.Local, E.Descricao, E.Tipo, U.UserName
                            from Eventos E
                            inner Join UsuariosEventos UE on E.Id = UE.EventoId inner Join AspNetUsers U on UE.UsuarioId = U.Id
                            where UE.EventoId = @eventoId  
                        ";
                var evento = await connection.QueryAsync<Evento, User, Evento>(sql, MapResults, new { @eventoId = eventoId }, splitOn: "UserName");

                return evento.ToList();
            }
        }

        private Evento MapResults (Evento evento, User user){
                    evento.User = user;
                    return evento;
        }

         public List<Evento> GetAllEventosParticipando(int usuarioId)
        {
            
            using (SqlConnection connection = new SqlConnection(_conectionString))
                    {
                        var sql =@"select E.Id, E.DataEvento, E.Tema, E.Local, E.Descricao, E.Tipo
                                    from Eventos E
                                    inner Join UsuariosEventos UE on E.Id = UE.EventoId inner Join AspNetUsers U on UE.UsuarioId = U.Id
                                    where UE.UsuarioId = @userId
                                    
                                ";
                        var evento = connection.Query<Evento>(sql, new { @userId = usuarioId });

                        return evento.ToList();
                    } 
        }

        public List<Evento> GetAllEventosByData(string dataInicio, string dataFim)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var listaEventos = connection.Query<Evento>("select * from Eventos where DataEvento between @dataInicio and @dataFim", new { @dataInicio = dataInicio, @dataFim = dataFim} );
                return listaEventos.ToList();
            }
        }

        public List<Evento> GetAllEventosByTema(string tema)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                var listaEventos = connection.Query<Evento>("select * from Eventos where Tema like '%'+@tema+'%'", new { @tema = tema } );
                return listaEventos.ToList();
            }
        }
    

        public List<Evento> GetAllEventoUsuario(int usuarioId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {
                connection.Open();
                var listaEventos = connection.Query<Evento>("select * from Eventos where userId=@usuarioId", new { @usuarioId = usuarioId } );
                return listaEventos.ToList();
            }
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
        public async Task<User> GetUsuarioByIdAsync(int usuarioId)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT *      
                            From AspNetUsers
                            where Id = @Id
                        ";

                var usuario = connection.QueryMultiple(sql, new { @Id = usuarioId });
                var result = usuario.ReadSingle<User>();
                return result;  
            }
        }

        public async Task<User> GetUsuarioByNomeAsync(string nomeUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_conectionString))
            {     
                var sql = @"SELECT FullName      
                            From AspNetUsers
                            where FullName = @nome
                        ";

                var usuario = connection.QueryMultiple(sql, new { @nome = nomeUsuario });
                var result = usuario.ReadSingle<User>();
                return result;  
            }
        }
    }
}