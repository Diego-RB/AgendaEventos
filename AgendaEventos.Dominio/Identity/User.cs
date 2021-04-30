using System;
using System.Collections.Generic;
using AgendaEventos.Dominio.Identity;
using Microsoft.AspNetCore.Identity;

namespace AgendaEventos.Dominio.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string ImagemURL { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public List<Evento> Eventos { get; set; }
        public List<UsuarioEvento> UsuariosEventos { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}