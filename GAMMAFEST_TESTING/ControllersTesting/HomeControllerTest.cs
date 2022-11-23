using GAMMAFEST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using GAMMAFEST.Data;
using GAMMAFEST.Helpers;
using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Diagnostics;

namespace GAMMAFEST_TESTING.ControllersTesting
{
    public class HomeControllerTest
    {
        HomeController homeController;

        PromotorRepositorio promotorRepositorio;

        IQueryable lista1, lista2;

        string password = "TestPass";
        string testPrueba;
        byte[] pass;

        [SetUp]
        public void Setup()
        {
            testPrueba = HelperCryptography.GenerateSalt();

            lista1 = new List<Evento>()
            {
                new Evento{
                    IdUser = 1,
                    NombreEvento = "TestEvento",
                    AforoMaximo = 18,
                    NombreImagen = "image.jpg",
                    Descripcion = "Evento Test",
                    EventoId = 1,
                    FechaInicioEvento = new DateTime(2022, 10,18),
                    Protocolos = "Protocolos Evento Test",
                    PrecioEntradaUnit = 10
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
                new Evento{
                    IdUser = 1,
                    NombreEvento = "TestEvento3",
                    AforoMaximo = 0,
                    NombreImagen = "image.jpg",
                    Descripcion = "Evento Test3",
                    EventoId = 4,
                    FechaInicioEvento = new DateTime(2022, 10,18),
                    Protocolos = "Protocolos Evento Test",
                    PrecioEntradaUnit = 10
                },
            }.AsQueryable();

            var mockEvento = new Mock<DbSet<Evento>>();
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Provider).Returns(lista1.Provider);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.Expression).Returns(lista1.Expression);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.ElementType).Returns(lista1.ElementType);
            mockEvento.As<IQueryable<Evento>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<Evento>)lista1.GetEnumerator());
            var mock = new Mock<ContextoDb>();
            mock.Setup(o => o.Evento).Returns(mockEvento.Object);

            EventoRepositorio eventoRepositorio = new EventoRepositorio(mock.Object);

            //USER
            testPrueba = HelperCryptography.GenerateSalt();
            pass = HelperCryptography.EncriptarPassword(password, testPrueba);

            lista2 = new List<UserPromotor>
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
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.Provider).Returns(lista2.Provider);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.Expression).Returns(lista2.Expression);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.ElementType).Returns(lista2.ElementType);
            mockUser.As<IQueryable<UserPromotor>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<UserPromotor>)lista2.GetEnumerator());

            var mock2 = new Mock<ContextoDb>();
            mock2.Setup(o => o.UserPromotor).Returns(mockUser.Object);


            //CLAIMS

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { new Claim(ClaimTypes.Name, "UserTest") });
            var mockContext = new Mock<IHttpContextAccessor>();
            mockContext.Setup(o => o.HttpContext.User).Returns(mockClaimsPrincipal.Object);

            promotorRepositorio = new PromotorRepositorio(mock2.Object, mockContext.Object);

            homeController = new HomeController(eventoRepositorio, promotorRepositorio);
        }
        
        [Test]
        public void IndexTest() {

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { new Claim(ClaimTypes.Name, "User") });
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

            var result = homeController.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void PrivacyTest()
        {
            var result = homeController.Privacy();
            Assert.IsNotNull(result);
        }
    }
}
