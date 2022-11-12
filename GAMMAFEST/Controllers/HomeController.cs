using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GAMMAFEST.Controllers
{
    public interface IHomeController { 
        public IActionResult Index();
    }
    public class HomeController : Controller
    {
        bool temp = false;
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
            if (User.Identity.IsAuthenticated && rep.ConteoUser() > 0)
            {
                ViewBag.estado = GetLoggedUser().tipoUsuario;
                ViewBag.perfil = GetLoggedUser().IdUser.ToString();
            }
            IEnumerable<Evento> citaEvento = repositorio.ObtenerTodosEventos();
            return View(citaEvento);
        }

        public UserPromotor GetLoggedUser()
        {
            var claim = User.Claims.FirstOrDefault();
            var username = (claim == null ? string.Empty : claim.Value);
            return rep.ObtenerInicio(username);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}