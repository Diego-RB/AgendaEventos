using System.ComponentModel.DataAnnotations;

namespace AgendaEventos.API.Dtos
{
    public class UsuarioEventoDto
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int EventoId { get; set; }
    }
}