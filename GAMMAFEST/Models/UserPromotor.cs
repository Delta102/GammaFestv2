using System.ComponentModel.DataAnnotations;

namespace GAMMAFEST.Models
{
    public class UserPromotor
    {
        [Key]
        public int IdUser { get; set; }
        public byte[] Contrasenia { get; set; }
        public string Email { get; set; }
        public string Cifrado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string tipoUsuario { get; set; }
    }
}