using GAMMAFEST.Data;
using GAMMAFEST.Models;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace GAMMAFEST.Repositorio
{
    public interface IEntradaRepositorio {
        void CrearEntrada(Entrada entrada);
        //void GuardarEnlace(Qr qr);
        IEnumerable<Entrada> HistorialEntradas(int id);
        IEnumerable<Entrada> HistorialCantidad(int id, int idCant);
        int ConteoEntrada();
        int ConteoEntradaByEventoId(int id);
    }
    public class EntradaRepositorio: IEntradaRepositorio
    {
        public readonly ContextoDb _context;
        public EntradaRepositorio(ContextoDb context)
        {
            _context = context;
        }
        public void CrearEntrada(Entrada entrada)
        {
            _context.Add(entrada);
            _context.SaveChanges();
        }

        public int ConteoEntrada() {
            return _context.Entrada.Count();
        }

        public int ConteoEntradaByEventoId(int id)
        {
            return _context.Entrada.Where(u => u.EventoId==id).Count();
        }

        public IEnumerable<Entrada> HistorialEntradas(int id) {
            IEnumerable<Entrada> lista=_context.Entrada.Include(e=>e.Evento).Where(e => e.IdUser == id).Where(e=>e.EntradaId == e.IdCantidad);
            return lista;
        }

        public List<Entrada> EntradaconNombreEvento(int id)
        {
            List<Entrada> lista = _context.Entrada.Include(e => e.Evento).Where(e => e.IdUser == id).ToList();
            return lista;   
        }

        public IEnumerable<Entrada> HistorialCantidad(int id, int idCant)
        {
            IEnumerable<Entrada> lista = _context.Entrada.Include(e => e.Evento).Where(e => e.IdUser == id).Where(u=>u.IdCantidad == idCant);
            return lista;
        }
    }
}