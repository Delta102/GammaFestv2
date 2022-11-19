using GAMMAFEST.Controllers;
using GAMMAFEST.Data;
using GAMMAFEST.Helpers;
using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAMMAFEST_TESTING.ControllersTesting
{
    public class EventoControllerTest
    {
        EventoController controller;
        EventoRepositorio eRepositorio;
        IQueryable lista;
        string testPrueba;
        string password = "TestPass";
        byte[] pass;

        [SetUp]
        public void Setup()
        {
            var content = "Hello World from a Fake File";
            var fileName = "image.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            testPrueba = HelperCryptography.GenerateSalt();
            pass = HelperCryptography.EncriptarPassword(password, testPrueba);

            lista = new List<Evento>()
            {
                new Evento{
                    IdUser = 1,
                    NombreEvento = "TestEvento",
                    AforoMaximo = 18,
                    NombreImagen = "image.jpg",
                    Descripcion = "Evento Test",
                    EventoId = 1,
                    ArchivoImagen = new FormFile (stream, 0, stream.Length, "id_from_form", fileName),
                    FechaInicioEvento = new DateTime(2022, 10,18),
                    Protocolos = "Protocolos Evento Test",
                    PrecioEntradaUnit = 10,
                    UserPromotor =new UserPromotor{
                        IdUser = 1,
                        Email = "test@test.pe",
                        Nombre = "Test",
                        Apellido = "Test2",
                        Cifrado = testPrueba,
                        Contrasenia = HelperCryptography.EncriptarPassword("TestPass", testPrueba),
                        tipoUsuario="ADMIN"
                    }
                },

                new Evento{
                    IdUser = 1,
                    NombreEvento = "TestEvento2",
                    AforoMaximo = 3,
                    NombreImagen = "image.jpg",
                    Descripcion = "Evento Test2",
                    PrecioEntradaUnit = 10,
                    EventoId = 2,
                    FechaInicioEvento = new DateTime(2022, 10,18),
                    Protocolos = "Protocolos Evento Test"
                },

                new Evento{
                    IdUser = 1,
                    NombreEvento = "TestEvento3",
                    AforoMaximo = 3,
                    NombreImagen = "image.jpg",
                    Descripcion = "Evento Test3",
                    EventoId = 3,
                    FechaInicioEvento = new DateTime(2022, 10,18),
                    Protocolos = "Protocolos Evento Test",
                    PrecioEntradaUnit = 10
                },
            }.AsQueryable();

            var mockEvento = new Mock<DbSet<Evento>>();
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Provider).Returns(lista.Provider);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Expression).Returns(lista.Expression);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.ElementType).Returns(lista.ElementType);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<Evento>)lista.GetEnumerator());
            var mock = new Mock<ContextoDb>();
            mock.Setup(o => o.Evento).Returns(mockEvento.Object);


             var webHostMock = new Mock<IWebHostEnvironment>();
            webHostMock.Setup(x => x.WebRootPath).Returns("E:\\CICLO VIII\\CALIDAD Y PRUEBAS DE SOFTWARE\\PROYECTO FINAL\\GAMMAFEST\\GAMMAFEST\\wwwroot\\");

            eRepositorio = new EventoRepositorio(mock.Object);
            controller = new EventoController(eRepositorio, webHostMock.Object);
        }

        // CREACIÓN DE EVENTO

        [Test]
        public void CrearEventoTest()
        {
            var result = controller.CrearEvento(1);
            Assert.IsInstanceOf<ViewResult>(result);


            var direccion = controller.CrearEvento(new Evento
            {
                IdUser = 1,
                NombreEvento = "TestEvento",
                AforoMaximo = 18,
                NombreImagen = "image.jpg",
                Descripcion = "Evento Test",
                EventoId = 1,
                FechaInicioEvento = new DateTime(2025, 10, 18),
                Protocolos = "Protocolos Evento Test",
                PrecioEntradaUnit = 10
            });

            Assert.IsInstanceOf<RedirectToActionResult>(direccion);
        }

        [Test]
        public void CrearEventoFailTest()
        {

            var direccionTemp = controller.CrearEvento(new Evento
            {
                IdUser = 1,
                NombreEvento = "TestEvento",
                AforoMaximo = 18,
                NombreImagen = "image.jpg",
                Descripcion = "Evento Test",
                EventoId = 1,
                FechaInicioEvento = new DateTime(2021, 10, 18),
                Protocolos = "Protocolos Evento Test",
                PrecioEntradaUnit = 10
            });

            Assert.IsInstanceOf<ViewResult>(direccionTemp);
        }

        [Test]
        public void IndexEventoTest() {

            var vista=controller.IndexEvento(1,1) as ViewResult;

            Assert.IsInstanceOf<Evento>(vista.Model);
        }

        [Test]
        public void SubirArchivoTest() {
            var evento = eRepositorio.ObtenerSoloEvento(1);
            var result = controller.SubirArchivo(evento);
            Assert.That(result, Is.EqualTo(evento.NombreImagen));
        }

        [Test]
        public void ListarEventosbyUserIdControllerTest() {
            var result = controller.ListarEventos(1) as ViewResult;
            Assert.IsInstanceOf<IEnumerable<Evento>>(result.Model);
        }



        //MÉTODOS REPOSITORIOS
        [Test]
        public void ObtenerTodosEventosTest()
        {
            var list = eRepositorio.ObtenerTodosEventos();
            Assert.That(list.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ObtenerSoloEventoTest()
        {
            var result = eRepositorio.ObtenerSoloEvento(1);
            Assert.That(result.NombreEvento, Is.EqualTo("TestEvento"));
        }

        [Test]
        public void ObtenerEventosByUserIdTest() {
            var result = eRepositorio.ObtenerTodosEventosByUserId(1);
            Assert.That(result.Count(), Is.EqualTo(3));

        }

        [Test]
        public void ObtenerSoloEventoConPromotorTest()
        {
            var result= eRepositorio.ObtenerSoloEventoConPromotor(1);
            Assert.That(result.UserPromotor.Email, Is.EqualTo("test@test.pe"));
        }
    }
}