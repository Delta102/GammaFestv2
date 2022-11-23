using GAMMAFEST.Data;
using GAMMAFEST.Models;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace GAMMAFEST.Repositorio
{
    public interface IRegistroAsistenciaRepositorio
    {
        void SubirQR(RegistroAsistencia registro);
        List<RegistroAsistencia> ListarAsistentes(List<Entrada> entradas);
        RegistroAsistencia ObtenerRegistro(int idEvento);
        List<RegistroAsistencia> registros();
        string Lectura(string archivo);
        string SubirArchivo(RegistroAsistencia qr);
    }
    public class RegistroAsistenciaRepositorio : IRegistroAsistenciaRepositorio
    {
        public readonly ContextoDb _context;
        public readonly IWebHostEnvironment _hostEnvironment;
        public RegistroAsistenciaRepositorio(ContextoDb context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

        public string Lectura(string archivo)
        {
            BarcodeResult QRResult = BarcodeReader.QuicklyReadOneBarcode(archivo);

            return QRResult.ToString();
        }

        public string SubirArchivo(RegistroAsistencia qr)
        {
            string? nArchivo = null;
            string? temp1 = null;
            if (qr.ArchivoImagen != null)
            {
                string path = _hostEnvironment.WebRootPath;
                nArchivo = Path.GetFileNameWithoutExtension(qr.ArchivoImagen.FileName);
                string x = Path.GetExtension(qr.ArchivoImagen.FileName);
                qr.NombreImagen = nArchivo + x;
                temp1 = nArchivo + x;
                string z = Path.Combine(path + "/imageQR/", nArchivo + x);

                if (!Directory.Exists(path + "/imageQR"))
                {
                    Directory.CreateDirectory(path + "/imageQR");
                }

                using (var fileStream = new FileStream(z, FileMode.Create))
                {
                    qr.ArchivoImagen.CopyTo(fileStream);
                }
            }
            return temp1;
        }
    }
}