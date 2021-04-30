using AgendaEventos.API.Dtos;
using AgendaEventos.Dominio;
using AgendaEventos.Dominio.Identity;
using AutoMapper;

namespace AgendaEventos.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<UsuarioEvento, UsuarioEventoDto>().ReverseMap();


        }
    }
}