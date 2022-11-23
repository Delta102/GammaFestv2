using GAMMAFEST.Filters;
using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace GAMMAFEST.Controllers
{
    public class EventoController : Controller
    {
        public readonly IEventoRepositorio repositorio;
        public readonly IWebHostEnvironment _hostEnvironment;

        public EventoController(IEventoRepositorio repositorio, IWebHostEnvironment hostEnvironment)
        {
            this.repositorio = repositorio;
            _hostEnvironment = hostEnvironment;
        }

        [AuthorizeUsers(Policy ="ADMINISTRADORES")]
        public IActionResult CrearEvento(int id) {

            ViewBag.fechaNow = DateTime.Now;
            ViewBag.idPromTemp = id;
            ViewData["Verificado"] = "Encontrado";
            return View();
        }
        public string SubirArchivo(Evento evento)
        {
            string? nArchivo = null;
            string? temp1 = null;
            if (evento.ArchivoImagen != null)
            {
                string path = _hostEnvironment.WebRootPath;
                nArchivo = Path.GetFileNameWithoutExtension(evento.ArchivoImagen.FileName);
                string x = Path.GetExtension(evento.ArchivoImagen.FileName);
                evento.NombreImagen = nArchivo + x;
                temp1 = nArchivo + x;
                string z = Path.Combine(path + "/imageBD/", nArchivo + x);

                if (!Directory.Exists(path+"/imageBD"))
                {
                    Directory.CreateDirectory(path + "/imageBD");
                }

                using (var fileStream = new FileStream(z, FileMode.Create))
                {
                    evento.ArchivoImagen.CopyTo(fileStream);
                }
            }
            return temp1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearEvento(Evento evento) {
            if (ModelState.IsValid)
            {
                if (evento.FechaInicioEvento <= DateTime.Now)
                    ViewData["MENSAJE"] = "La fecha ingresada no puede ser menor a la fecha actual";
                
                else
                {
                    evento.NombreImagen = SubirArchivo(evento);
                    //evento.UserPromotor = promotorRepositorio.ObtenerUserById(id);
                    repositorio.CrearEvento(evento);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(evento);
        }

        [AuthorizeUsers]
        [HttpGet]
        public IActionResult IndexEvento(int? id, int id2)
        {
            var evento = repositorio.ObtenerSoloEventoConPromotor((int)id);
            ViewBag.idTemp = id2.ToString();
            return View(evento);
        }


        [HttpGet]
        public IActionResult ListarEventos(int temp, int idUser) {
            List<Evento> list = new List<Evento>();
            switch (temp) {
                case 1: list = repositorio.ObtenerTodosEventosByUserIdPasados(idUser, 1); break;

                case 2: list = repositorio.ObtenerTodosEventosByUserIdPasados(idUser, 2); break;

                case 3: list = repositorio.ObtenerTodosEventosByUserIdPasados(idUser, 3); break;
            }

            ViewBag.temp = temp;
            ViewBag.id = idUser;
            return View(list);
        }
    }
}