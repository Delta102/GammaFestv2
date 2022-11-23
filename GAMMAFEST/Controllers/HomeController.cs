using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GAMMAFEST.Controllers
{
    public class HomeController : Controller
    {
        public readonly IEventoRepositorio repositorio;
        public readonly IPromotorRepositorio rep;

        public HomeController(IEventoRepositorio repositorio, IPromotorRepositorio rep)
        {
            this.rep = rep;
            this.repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = rep.GetLoggedUser();
            if (user != null) {
                ViewBag.estado = rep.GetLoggedUser().tipoUsuario;
                ViewBag.perfil = rep.GetLoggedUser().IdUser.ToString();
            }

            IEnumerable<Evento> citaEvento = repositorio.ObtenerTodosEventos();
            return View(citaEvento);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}