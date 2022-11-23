using GAMMAFEST.Data;
using GAMMAFEST.Models;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace GAMMAFEST.Repositorio
{
    public interface IEntradaRepositorio {
        void CrearEntrada(Entrada entrada);
        //void GuardarEnlace(Qr qr);
        Entrada ObtenerEntrada(int id);
        IEnumerable<Entrada> HistorialEntradas(int id);
        IEnumerable<Entrada> HistorialCantidad(int id, int idCant);
        List<Entrada> ObtenerEntradasByEventoId(int idEvento);
        int ConteoEntrada();
        int ConteoEntradaByEventoId(int id);
        void generarQR(Entrada entrada, Evento evento, UserPromotor user, string webPath);
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

        public void generarQR(Entrada entrada, Evento evento, UserPromotor user,  string webPath)
        {
            var txtQR = "#_de_Ticket:_" + entrada.EntradaId + "~_Nombre_de_Evento:_" + evento.NombreEvento + "~_Id_de_Evento:_" + evento.EventoId+ "~_Usuario:_" + user.Nombre + "~_Id_de_Usuario:_" + user.IdUser;

            var qr = QRCodeWriter.CreateQrCode(txtQR);

            string path = Path.Combine(webPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, "QRnumber" + entrada.EntradaId + ".png");
            qr.SaveAsPng(filePath);
        }

        public Entrada ObtenerEntrada(int id) {
            return _context.Entrada.Single(u=>u.EntradaId == id);
        }

        public List<Entrada> ObtenerEntradasByEventoId(int idEvento)
        {
            return _context.Entrada.Where(u => u.EventoId == idEvento).ToList();
        }
    }
}