using GAMMAFEST.Data;
using GAMMAFEST.Models;
using Microsoft.EntityFrameworkCore;

namespace GAMMAFEST.Repositorio
{
    public interface IEventoRepositorio
    {
        IEnumerable<Evento> ObtenerTodosEventos();
        IEnumerable<Evento> ObtenerTodosEventosByUserId(int id);
        List<Evento> ObtenerTodosEventosByUserIdPasados(int id, int temp);
        Evento ObtenerSoloEvento(int id);
        Evento ObtenerSoloEventoConPromotor(int id);
        void CrearEvento(Evento evento);
        void ActualizarEvento(Evento evento);
    }
    public class EventoRepositorio: IEventoRepositorio
    {
        public readonly ContextoDb _context;
        public EventoRepositorio(ContextoDb context) {
            _context = context;
        }

        public IEnumerable<Evento> ObtenerTodosEventos()
        {
            return _context.Evento;
        }

        public IEnumerable<Evento> ObtenerTodosEventosByUserId(int id)
        {
            return _context.Evento.Include(o => o.UserPromotor).Where(o=>o.IdUser == id);
        }

        public List<Evento> ObtenerTodosEventosByUserIdPasados(int id, int temp)
        {
            if (temp == 1)
                return _context.Evento.Where(u=>u.FechaInicioEvento < DateTime.Now).Where(u=>u.IdUser == id).ToList();

            if (temp == 2)
                    return _context.Evento.Where(u => u.FechaInicioEvento >= DateTime.Now && u.FechaInicioEvento <= DateTime.Now.AddMinutes(30)).Where(u => u.IdUser == id).ToList();

            if (temp == 3)
                return _context.Evento.Where(u => u.FechaInicioEvento > DateTime.Now.AddMinutes(30)).Where(u => u.IdUser == id).ToList();

            return null;
        }

        public Evento ObtenerSoloEvento(int id)
        {
            return _context.Evento.Single(e=>e.EventoId==id);
        }

        public Evento ObtenerSoloEventoConPromotor(int id)
        {
            return _context.Evento.Include(p => p.UserPromotor).Single(e => e.EventoId == id);
        }

        public void CrearEvento(Evento evento) {
            _context.Add(evento);
            _context.SaveChangesAsync();
        }

        public void ActualizarEvento(Evento evento)
        {
            _context.Update(evento);
            _context.SaveChanges();
        }
    }
}