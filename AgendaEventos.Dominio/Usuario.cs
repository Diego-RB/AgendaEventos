using System;
using System.Collections.Generic;

namespace AgendaEventos.Dominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public string Senha { get; set; }
        public List<Evento> Eventos { get; set; }
        public List<UsuarioEvento> UsuariosEventos { get; set; }
    }
}