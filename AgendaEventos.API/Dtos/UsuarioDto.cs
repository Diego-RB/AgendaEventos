namespace AgendaEventos.API.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public char Sexo { get; set; }
        public string Senha { get; set; }
    }
}