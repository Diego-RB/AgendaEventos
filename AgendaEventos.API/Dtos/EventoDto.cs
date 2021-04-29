using System.ComponentModel.DataAnnotations;

namespace AgendaEventos.API.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Local deve ter entre 3 e 100 Caracters")]
        public string Local { get; set; }
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "O Tema deve ser preenchido")]
        public string Tema { get; set; }
        //[Range(2,120000, ErrorMessage = "Quantidade de Pessoas deve ser entre 2 e 120000")]
        [Required(ErrorMessage = "A Descrição deve ser preenchido")]
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }


    }
}