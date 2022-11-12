﻿using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GAMMAFEST.Controllers
{
    public class PromotorController : Controller
    {
        private IPromotorRepositorio rep;

        public PromotorController(IPromotorRepositorio rep)
        {
            this.rep = rep;
        }

        //REGISTRO DE PROMOTORES
        
        public IActionResult RegistroPromotor() {
            return View();
        }

        [HttpPost]
        public IActionResult RegistroPromotor(string email, string password, string nombre, string apellidos, string tipo) {

            bool registrado = this.rep.RegistrarUsuario(email, password, nombre, apellidos, tipo);
            if (registrado)
                ViewData["MENSAJE"] = "Usuario registrado con exito";
            else 
                ViewData["MENSAJE"] = "Lo sentimos, el correo electrónico ya existe en nuestra base de datos";
            
            return View();
        }

        //
        //
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            UserPromotor promotor = this.rep.LogInUsuario(email, password);
            if (promotor == null)
            {
                ViewData["MENSAJE"] = "No tienes credenciales correctas";
                return View();
            }
            else
            {
                //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                //Y UN PRINCIPAL
                //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                //AUTENTIFICACION
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                Claim claimUserName = new Claim(ClaimTypes.Name, promotor.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, promotor.tipoUsuario);
                Claim claimIdUsuario = new Claim("IdUser", promotor.IdUser.ToString());
                Claim claimEmail = new Claim("EmailUsuario", promotor.Email);

                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });

                return RedirectToAction("Index", "Home", new {id=promotor.IdUser});
            }

        }
        [HttpGet]
        public IActionResult Init()
        {
            ViewBag.perfil = GetLoggedUser();

            return View();
        }
        public UserPromotor GetLoggedUser()
        {
            var claim = User.Claims.FirstOrDefault();
            var username = claim.Value;
            return rep.ObtenerInicio(username);
        }

        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
