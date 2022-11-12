using GAMMAFEST.Models;
using Microsoft.EntityFrameworkCore;

namespace GAMMAFEST.Data
{
    public class ContextoDb: DbContext
    {
        public ContextoDb() { }
        public ContextoDb(DbContextOptions<ContextoDb> options) : base(options)
        {
        }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<UserPromotor> UserPromotor { get; set; }
        public virtual DbSet<Entrada> Entrada { get; set; }
        //public virtual DbSet<Qr> Qr { get; set; }
    }
}
