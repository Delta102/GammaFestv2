using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMMAFEST.Models
{
    public class Comentario
    {
        [Key]
        public int IdComentario {get; set;}
        public string Descripcion {get; set;}
        [DataType(DataType.Date)]
        public DateTime FechaComentario { get; set;}
        [ForeignKey("IdUser")]
        public virtual UserPromotor? UserPromotor {get; set;}
        [ForeignKey("EventoId")]
        public virtual Evento? Evento {get; set;}
    }
}
