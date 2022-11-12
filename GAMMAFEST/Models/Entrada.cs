using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMMAFEST.Models
{
    public class Entrada
    {
        [Key]
        public int EntradaId { get; set; }
        public int CantidadEntradas { get; set; }
        public int EventoId { get; set; }
        public int IdUser { get; set; }
        public int PrecioTotal { get; set; }
        public string TextoQR { get; set; }
        public int IdCantidad { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento? Evento { get; set; }
    }
}