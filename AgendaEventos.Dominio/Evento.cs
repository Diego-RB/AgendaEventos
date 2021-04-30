using System;
using System.Collections.Generic;
using AgendaEventos.Dominio.Identity;

namespace AgendaEventos.Dominio
{
    public class Evento
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime DataEvento { get; set; }
        public string Tema { get; set; }
        public string Descricao { get; set; }
        public char Tipo { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<UsuarioEvento> UsuariosEventos { get; set; }
    }
}