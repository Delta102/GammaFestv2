using GAMMAFEST.Controllers;
using GAMMAFEST.Data;
using GAMMAFEST.Helpers;
using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SixLabors.ImageSharp;
using System.IO;
using System.Security.Claims;

namespace GAMMAFEST_TESTING.ControllersTesting
{
    public class PromotorControllerTest
    {
        PromotorController controller;
        PromotorRepositorio eRepositorio;
        EventoRepositorio eventoRepositorio;

        IQueryable lista, listaEvento;
        string password = "TestPass";
        string testPrueba;
        byte[] pass;

        [SetUp]
        public void Setup() {
            var content = "Hello World from a Fake File";
            var fileName = "image.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            testPrueba = HelperCryptography.GenerateSalt();
            pass = HelperCryptography.EncriptarPassword(password, testPrueba);
            lista = new List<UserPromotor>
            {
                new UserPromotor{
                    IdUser = 1,
                    Email = "Usertest@user.pe",
                    Nombre = "UserTest",
                    Apellido = "UserTest",
                    Cifrado = testPrueba,
                    Contrasenia = pass,
                    tipoUsuario="USUARIO"
                },

                new UserPromotor{
                    IdUser = 2,
                    Email = "Admintest@admin.pe",
                    Nombre = "Test",
                    Apellido = "Test2",
                    Cifrado = testPrueba,
                    Contrasenia = HelperCryptography.EncriptarPassword("TestPass", testPrueba),
                    tipoUsuario="ADMIN"
                }

            }.AsQueryable();

            var mockUser = new Mock<DbSet<UserPromotor>>();
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.Provider).Returns(lista.Provider);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.Expression).Returns(lista.Expression);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.ElementType).Returns(lista.ElementType);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<UserPromotor>)lista.GetEnumerator());

            var mock = new Mock<ContextoDb>();
            mock.Setup(o => o.UserPromotor).Returns(mockUser.Object);


            listaEvento = new List<Evento>()
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
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Provider).Returns(listaEvento.Provider);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Expression).Returns(listaEvento.Expression);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.ElementType).Returns(listaEvento.ElementType);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<Evento>)listaEvento.GetEnumerator());

            var mockEventoDb = new Mock<ContextoDb>();
            mockEventoDb.Setup(o => o.Evento).Returns(mockEvento.Object);

            eventoRepositorio = new EventoRepositorio(mockEventoDb.Object);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { new Claim(ClaimTypes.Name, "User1") });
            var mockContext = new Mock<IHttpContextAccessor>();
            mockContext.Setup(o => o.HttpContext.User).Returns(mockClaimsPrincipal.Object);

            eRepositorio = new PromotorRepositorio(mock.Object, mockContext.Object);

            controller = new PromotorController(eRepositorio, eventoRepositorio);
        }

        //PRUEBAS CON EL CONTROLADOR
        [Test]
        public void RegistroPromotorControllerTest() {
            var result = controller.RegistroPromotor("test1@test.pe", "testPass", "TestUnit", "TestUnit", "ADMIN") as ViewResult;
            Assert.That(result.ViewData["MENSAJE"], Is.EqualTo("Usuario registrado con exito"));
        }

        [Test]
        public void RegistroPromotorFailControllerTest()
        {
            var result = controller.RegistroPromotor("Usertest@user.pe", "testPass", "TestUnit", "TestUnit", "ADMIN") as ViewResult;
            Assert.That(result.ViewData["MENSAJE"], Is.EqualTo("Lo sentimos, el correo electrónico ya existe en nuestra base de datos"));
        }

        [Test]
        public void LoginTest()
        {
            var view = controller.Login();
            var result = controller.Login("Usertest@user.pe", password);

            Assert.IsNotNull(view);
            Assert.That(controller.ViewData["MENSAJE"], Is.EqualTo(null));

            Assert.IsInstanceOf<Task<IActionResult>>(result);
        }
        [Test]
        public void LoginFailTest()
        {
            var controller = new PromotorController(eRepositorio, eventoRepositorio);
            var result = controller.Login("Usertest@user.pe", password + "asd");
            Assert.That(controller.ViewData["MENSAJE"], Is.EqualTo("No tienes credenciales correctas"));
            Assert.IsInstanceOf<Task<IActionResult>>(result);
        }

        [Test]
        public void LoginUserNullTest()
        {
            var result = controller.Login("Usertest6@user6.pe", password + "asd");
            Assert.That(controller.ViewData["MENSAJE"], Is.EqualTo("No tienes credenciales correctas"));
            Assert.IsInstanceOf<Task<IActionResult>>(result);
        }

        [Test]
        public void RegistroViewTest()
        {
            var view = controller.RegistroPromotor();
            Assert.IsNotNull(view);
        }
 
        [Test]
        public void ObtenerUsuarioLogueadoTest() {
            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { new Claim(ClaimTypes.Name, "User1") });
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

            var user = controller.Init();
            Assert.IsNotNull(user);
        }


        [Test]
        public void CerrarSesionTest()
        {
            var direccion = controller.LogOut();
            Assert.IsInstanceOf<Task<IActionResult>>(direccion);
        }


        //PRUEBAS CON EL REPOSITORIO
        [Test]
        public void RegistroPromotorRepositorioTest()
        {
            bool result = eRepositorio.RegistrarUsuario("Usertest1@user.pe", "temp", "user", "user", "Usuario");
            Assert.IsTrue(result);
        }
        [Test]
        public void RegistroPromotorFailRepositorioTest()
        {
            bool result = eRepositorio.RegistrarUsuario("Usertest@user.pe", "temp", "user", "user", "Usuario");
            Assert.IsFalse(result);
        }

        [Test]
        public void ObtenerUsuarioLogueado()
        {
            var result = eRepositorio.ObtenerInicio("Test");
            Assert.That(result.Nombre, Is.EqualTo("Test"));
        }

        [Test]
        public void ObtenerUserByIdTest() {
            var result = eRepositorio.ObtenerUserById(1);
            Assert.That(result.IdUser, Is.EqualTo(1));
            Assert.That(result.Nombre, Is.EqualTo("UserTest"));
        }
        [Test]
        public void ConteoUserTest()
        {
            var result = eRepositorio.ConteoUser();
            Assert.That(result, Is.EqualTo(2));
        }
    }
}