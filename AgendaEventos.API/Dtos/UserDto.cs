using System.ComponentModel.DataAnnotations;

namespace AgendaEventos.API.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
         public string DataNascimento { get; set; }
        public char Sexo { get; set; }
    }
}