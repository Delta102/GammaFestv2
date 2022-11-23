using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts.Tables.TrueType;

namespace GAMMAFEST.Controllers
{
    public class RegistroAsistenciaController : Controller
    {
        bool encontrado = false;
        public readonly IRegistroAsistenciaRepositorio repositorio;
        public readonly IPromotorRepositorio promotorRepositorio;
        public readonly IEntradaRepositorio entradaRepositorio;
        readonly IEventoRepositorio eventoRepositorio;

        public readonly IWebHostEnvironment _hostEnvironment;

        public RegistroAsistenciaController(IRegistroAsistenciaRepositorio repositorio,  IWebHostEnvironment hostEnvironment, IPromotorRepositorio promotorRepositorio, IEventoRepositorio eventoRepositorio, IEntradaRepositorio entradaRepositorio)
        {
            this.repositorio = repositorio;
            _hostEnvironment = hostEnvironment;
            this.promotorRepositorio = promotorRepositorio;
            this.eventoRepositorio = eventoRepositorio;
            this.entradaRepositorio = entradaRepositorio;
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

        public IActionResult LeerQR()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LeerQR(RegistroAsistencia qr, int idEvento)
        {
            if (ModelState.IsValid)
            {
                qr.NombreImagen = SubirArchivo(qr);

                var path = _hostEnvironment.WebRootPath;

                var fullPath = path + "\\imageQR\\"+qr.NombreImagen;

                string s = Lectura(fullPath);

                string[] subs = s.Split('~');

                var ticketId = subs[0].Remove(0, 13);


                /*var txtEvento = subs[2].Remove(0, 7).Replace("_", " ");

                var txtUsuario = subs[3].Remove(0, 10).Replace("_", " ");*/

                qr.EntradaId = int.Parse(ticketId);

                var entrada = entradaRepositorio.ObtenerEntrada(qr.EntradaId);

                var registros = repositorio.registros();

                var evento = eventoRepositorio.ObtenerSoloEvento(entrada.EventoId);

                var user = promotorRepositorio.ObtenerUserById(entrada.IdUser);


                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                if (idEvento != evento.EventoId)
                    ViewData["MENSAJE"] = "Lo sentimos, el QR no pertenece a este evento";
                else
                {
                    for (int i = 0; i < registros.Count(); i++)
                        if (qr.EntradaId == registros[i].EntradaId)
                        {
                            ViewData["MENSAJE"] = "Lo sentimos, su entrada ya ha sido registrada";
                            encontrado = false;
                        }
                        else
                            encontrado = true;

                    if (encontrado) {
                        ViewData["MENSAJE"] = "Felicidades, la asistencia fue registrada con éxito";
                        repositorio.SubirQR(qr);
                    }
                }

                //var tempString = 

                ViewBag.ticket = qr.EntradaId;
                ViewBag.evento = evento.NombreEvento;
                ViewBag.usuario = user.Nombre;


            }
            return View(qr);
        }


        public string Lectura(string archivo)
        {
            BarcodeResult QRResult = BarcodeReader.QuicklyReadOneBarcode(archivo);

            return QRResult.ToString();
        }

        [HttpGet]
        public IActionResult ListaAsistentes(int idEvento) {
            var evento = eventoRepositorio.ObtenerSoloEvento(idEvento);
            var entrada = entradaRepositorio.ObtenerEntradasByEventoId(evento.EventoId);

            var registro = repositorio.ListarAsistentes(entrada);


            return View(registro);
        }
    }
}