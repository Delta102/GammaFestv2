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
        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<UserPromotor> UserPromotor { get; set; }
        public virtual DbSet<Entrada> Entrada { get; set; }
        public virtual DbSet<RegistroAsistencia> RegistroAsistencia { get; set; }
    }
}