using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing;
using Path = System.IO.Path;

namespace GAMMAFEST.Controllers
{
    public class EntradaController : Controller
    {
        public IEventoRepositorio eventoRepositorio;
        public IEntradaRepositorio repositorioEntrada;
        public IPromotorRepositorio repositorio;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EntradaController(IPromotorRepositorio repositorio, IEntradaRepositorio repositorioEntrada, IWebHostEnvironment webHostEnvironment, IEventoRepositorio eventoRepositorio)
        {
            this.eventoRepositorio = eventoRepositorio;
            this.repositorioEntrada = repositorioEntrada;
            this.repositorio = repositorio;
            _hostEnvironment = webHostEnvironment;
        }
        
        public IActionResult VentaEntrada(int idUser, int idEvento) {

            ViewBag.Count = repositorioEntrada.ConteoEntrada()+1;
            UserPromotor user = repositorio.ObtenerUserById(idUser);


            ViewBag.nombre = user.Nombre;
            ViewBag.apellido = user.Apellido;

            ViewBag.idEvento = idEvento;
            ViewBag.idPromotor = idUser;

            var evento = eventoRepositorio.ObtenerSoloEvento(idEvento);

            /**/


            if (evento.AforoMaximo>0)
            {
                if(evento.AforoMaximo>=1 && evento.AforoMaximo <= 4)
                    ViewBag.contadorAforo = evento.AforoMaximo;
                if (evento.AforoMaximo>10)
                    ViewBag.contadorAforo = 4;
                if (evento.AforoMaximo > 4 && evento.AforoMaximo<=10)
                    ViewBag.contadorAforo = 4;
                ViewBag.precio = eventoRepositorio.ObtenerSoloEvento(idEvento).PrecioEntradaUnit;
                return View();
            }
            else
                return RedirectToAction("SoldOut", "Entrada");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VentaEntrada(Entrada entrada, int idEvento, int idUser) {

            if (idEvento == eventoRepositorio.ObtenerSoloEvento(idEvento).EventoId && ModelState.IsValid) {
                var cantidadUser = repositorioEntrada.ConteoEntradaByEventoId(idEvento);
                var evento = eventoRepositorio.ObtenerSoloEvento(idEvento);

                var user = repositorio.ObtenerUserById(idUser);
                evento.AforoMaximo -= entrada.CantidadEntradas;
                if (cantidadUser > 0)
                    eventoRepositorio.ActualizarEvento(evento);

                var montoTotal = entrada.CantidadEntradas * eventoRepositorio.ObtenerSoloEvento(entrada.EventoId).PrecioEntradaUnit;
                entrada.PrecioTotal = (int)montoTotal;
                entrada.TextoQR = "Nombres: " + user.Nombre + "\n Apellidos: " + user.Apellido + " Evento: " + evento.EventoId + " Nombre del Evento: " + evento.NombreEvento;

                //Añadir Información

                for (int i = 0; i < entrada.CantidadEntradas; i++)
                {
                    var entradaTemp = new Entrada {
                        PrecioTotal = entrada.PrecioTotal,
                        TextoQR = entrada.TextoQR,
                        EventoId = idEvento,
                        IdUser = idUser,
                        CantidadEntradas = entrada.CantidadEntradas,
                        IdCantidad = entrada.EntradaId
                    };
                    repositorioEntrada.CrearEntrada(entradaTemp);
                    generarQR(entradaTemp);
                }
                //CORREGIR SISTEMA DE ENTRADAS
                //repositorioEntrada.CrearEntrada(entrada, entrada.CantidadEntradas);
                return RedirectToAction("HistorialCantidad", "Entrada", new {id = idUser, idCant = entrada.IdCantidad});
            }
            else
                return View(entrada);
        }

        private void generarQR(Entrada entrada) {
            var nom = eventoRepositorio.ObtenerSoloEvento(entrada.EventoId).NombreEvento;
            var user = repositorio.ObtenerUserById(entrada.IdUser).Nombre;

            var txtQR = "# de Ticket: " + entrada.EntradaId + "\nNombre de Evento: " + nom + "\nUsuario: " + user;

            var logoPath = Path.Combine(_hostEnvironment.WebRootPath, "image\\");

            var qr = QRCodeWriter.CreateQrCodeWithLogo(txtQR, logoPath+"logo_black_qrLogo.jpg");
            
            string path = Path.Combine(_hostEnvironment.WebRootPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "GeneratedQRCode/qrcodev2" + entrada.EntradaId + ".png");
            qr.SaveAsPng(filePath);

        }

        public IActionResult SoldOut()
        {
            return View();
        }

        public IActionResult Qr(string texto, int id)
        {
            try
            {
                ViewBag.EntradaId = id;
                //repositorioEntrada.GuardarEnlace(qr);
                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(texto, 200);
                barcode.AddBarcodeValueTextBelowBarcode();
                // Styling a QR code and adding annotation text
                barcode.SetMargins(10);
                barcode.ChangeBarCodeColor(Color.Black);
                string path = Path.Combine(_hostEnvironment.WebRootPath, "GeneratedQRCode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "GeneratedQRCode/qrcodev2" + id + ".png");
                barcode.SaveAsPng(filePath);
                string fileName = Path.GetFileName(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                ViewBag.QrCodeUri = imageUrl;
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        [HttpGet]
        public IActionResult HistorialEntrada(int id) {
            IEnumerable<Entrada> entrada = repositorioEntrada.HistorialEntradas(id);
            return View(entrada);
            
        }

        [HttpGet]
        public IActionResult HistorialCantidad(int id, int idCant)
        {
            IEnumerable<Entrada> entrada = repositorioEntrada.HistorialCantidad(id, idCant);
            return View(entrada);
        }
    }
}