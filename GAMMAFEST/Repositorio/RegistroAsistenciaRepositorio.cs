using GAMMAFEST.Data;
using GAMMAFEST.Models;
using Microsoft.EntityFrameworkCore;

namespace GAMMAFEST.Repositorio
{
    public interface IRegistroAsistenciaRepositorio
    {
        void SubirQR(RegistroAsistencia registro);
        List<RegistroAsistencia> ListarAsistentes(List<Entrada> entradas);
        RegistroAsistencia ObtenerRegistro(int idEvento);
        List<RegistroAsistencia> registros();
    }
    public class RegistroAsistenciaRepositorio : IRegistroAsistenciaRepositorio
    {
        public readonly ContextoDb _context;
        public RegistroAsistenciaRepositorio(ContextoDb context)
        {
            _context = context;
        }

        public void SubirQR(RegistroAsistencia registro) {
            _context.Add(registro);
            _context.SaveChanges();
        }

        public List<RegistroAsistencia> ListarAsistentes(List<Entrada> entradas) {
            RegistroAsistencia temp = new RegistroAsistencia();
            List<RegistroAsistencia> result = new List<RegistroAsistencia>();
            for (int i = 0; i < entradas.Count(); i++)
            {
                temp = _context.RegistroAsistencia.FirstOrDefault(u => u.EntradaId == entradas[i].EntradaId);
                if (temp != null)
                    result.Add(temp);
                temp = null;
            }

            return result;
        }

        public RegistroAsistencia ObtenerRegistro(int idEvento) {
            return _context.RegistroAsistencia.Single(u=>u.Id == idEvento);
        }

        public List<RegistroAsistencia> registros() { 
            return _context.RegistroAsistencia.ToList();
        }
    }
}