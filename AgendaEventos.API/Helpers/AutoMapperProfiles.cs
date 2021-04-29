using AgendaEventos.API.Dtos;
using AgendaEventos.Dominio;
using AutoMapper;

namespace AgendaEventos.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
            CreateMap<UsuarioEvento, UsuarioEventoDto>().ReverseMap();


        }
    }
}