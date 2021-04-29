using System;
using System.Collections.Generic;

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
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<UsuarioEvento> UsuariosEventos { get; set; }
    }
}