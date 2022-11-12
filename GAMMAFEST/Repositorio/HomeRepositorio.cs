using GAMMAFEST.Data;
using GAMMAFEST.Models;

namespace GAMMAFEST.Repositorio
{
    public class HomeRepositorio
    {
        /*public interface IHomeRepositorio{
            
        }

        public class EventoRepositorio : IHomeRepositorio
        {
            public readonly ContextoDb _context;
            public HomeRepositorio(ContextoDb context)
            {
                _context = context;
            }
            public IEnumerable<Evento> ObtenerEventos(int? id)
            {
                return _context.Evento.Include(p => p.Promotor).Where(e => e.EventoId == id); ;
            }

            public void CrearEvento(Evento evento)
            {
                _context.Add(evento);
                _context.SaveChangesAsync();
            }
        }*/

    }
}
