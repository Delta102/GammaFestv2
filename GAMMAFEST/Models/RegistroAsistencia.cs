using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMMAFEST.Models
{
    public class RegistroAsistencia
    {
        [Key]
        public int Id { get; set; }
        public string NombreImagen { get; set; }
        [NotMapped]
        public IFormFile ArchivoImagen { get; set; }
        public int EntradaId { get; set; }
        [ForeignKey("EntradaId")]
        public virtual Entrada? Entrada { get; set; }
    }
}