using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMMAFEST.Models
{
    public class Blog
    {
        [Key]
        public int IdBlog { get; set; }
        public string DescripcionBlog { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PublicacionBlog { get; set; }
        [ForeignKey("IdUser")]
        public virtual UserPromotor? UserPromotor { get; set; }
    }
}