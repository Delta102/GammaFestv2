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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GAMMAFEST_TESTING.ControllersTesting
{
    public class RegistroAsistenciaTest
    {
        RegistroAsistenciaController controller;

        RegistroAsistenciaRepositorio registroRepositorio;
        EventoRepositorio eventoRepositorio;
        PromotorRepositorio promotorRepositorio;
        EntradaRepositorio entradaRepositorio;


        IQueryable lista1, lista2, lista3, lista4;

        string password = "TestPass";
        string testPrueba;
        byte[] pass;

        [SetUp]
        public void Setup()
        {
            testPrueba = HelperCryptography.GenerateSalt();

            lista4 = new List<RegistroAsistencia>() {
                new RegistroAsistencia{
                    Id = 1,
                    NombreImagen = "image.jpg",
                    EntradaId= 1
                },

                new RegistroAsistencia{
                    Id = 2,
                    NombreImagen = "image2.jpg",
                    EntradaId= 1
                }
            }.AsQueryable();

            var mockRegistro = new Mock<DbSet<RegistroAsistencia>>();
            mockRegistro.As<IQueryable<RegistroAsistencia>>().Setup(o => o.Provider).Returns(lista4.Provider);
            mockRegistro.As<IQueryable<RegistroAsistencia>>().Setup(o => o.Expression).Returns(lista4.Expression);
            mockRegistro.As<IQueryable<RegistroAsistencia>>().Setup(o => o.ElementType).Returns(lista4.ElementType);
            mockRegistro.As<IQueryable<RegistroAsistencia>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<RegistroAsistencia>)lista4.GetEnumerator());
            var mockAsistencia = new Mock<ContextoDb>();
            mockAsistencia.Setup(o => o.RegistroAsistencia).Returns(mockRegistro.Object);


            var webHostMock = new Mock<IWebHostEnvironment>();
            webHostMock.Setup(x => x.WebRootPath).Returns("asd");

            registroRepositorio = new RegistroAsistenciaRepositorio(mockAsistencia.Object, webHostMock.Object);

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
                    PrecioEntradaUnit = 10,
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

            eventoRepositorio = new EventoRepositorio(mock.Object);

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



            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { new Claim(ClaimTypes.Name, "User1") });
            var mockContext = new Mock<IHttpContextAccessor>();
            mockContext.Setup(o => o.HttpContext.User).Returns(mockClaimsPrincipal.Object);

            /*controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object.HttpContext
            };*/

            promotorRepositorio = new PromotorRepositorio(mock2.Object, mockContext.Object);

            lista3 = new List<Entrada> {
                new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=1,
                    CantidadEntradas=4,
                    EventoId=1,
                    Evento = new Evento{
                        IdUser = 1,
                        NombreEvento = "TestEvento",
                        AforoMaximo = 18,
                        NombreImagen = "image.jpg",
                        Descripcion = "Evento Test",
                        EventoId = 5,
                        FechaInicioEvento = new DateTime(2022, 10,18),
                        Protocolos = "Protocolos Evento Test",
                        PrecioEntradaUnit = 10,
                    }
                },
                new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=2,
                    CantidadEntradas=4,
                    EventoId=1,
                    Evento = new Evento
                    {
                        IdUser = 1,
                        NombreEvento = "TestEvento",
                        AforoMaximo = 18,
                        NombreImagen = "image.jpg",
                        Descripcion = "Evento Test",
                        EventoId = 1,
                        FechaInicioEvento = new DateTime(2022, 10, 18),
                        Protocolos = "Protocolos Evento Test",
                        PrecioEntradaUnit = 10,
                        UserPromotor = new UserPromotor
                        {
                            IdUser = 1,
                            Email = "Usertest@user.pe",
                            Nombre = "UserTest",
                            Apellido = "UserTest",
                            Cifrado = testPrueba,
                            Contrasenia = pass,
                            tipoUsuario = "USUARIO"
                        }
                    }
                },
                new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=3,
                    CantidadEntradas=4,
                    EventoId=1
                },
                new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=4,
                    CantidadEntradas=4,
                    EventoId=1
                },
                new Entrada {
                    IdCantidad=5,
                    IdUser=2,
                    EntradaId=5,
                    CantidadEntradas=3,
                    EventoId=2,
                    Evento = new Evento{
                        IdUser = 2,
                        NombreEvento = "TestEvento2",
                        AforoMaximo = 18,
                        NombreImagen = "image.jpg",
                        Descripcion = "Evento Test",
                        EventoId = 6,
                        FechaInicioEvento = new DateTime(2022, 10,18),
                        Protocolos = "Protocolos Evento Test",
                        PrecioEntradaUnit = 10,
                    }
                },

            }.AsQueryable();
            var mockEntrada = new Mock<DbSet<Entrada>>();
            mockEntrada.As<IQueryable<Entrada>>().Setup(o => o.Provider).Returns(lista3.Provider);
            mockEntrada.As<IQueryable<Entrada>>().Setup(o => o.Expression).Returns(lista3.Expression);
            mockEntrada.As<IQueryable<Entrada>>().Setup(o => o.ElementType).Returns(lista3.ElementType);
            mockEntrada.As<IQueryable<Entrada>>().Setup(o => o.GetEnumerator()).Returns((IEnumerator<Entrada>)lista3.GetEnumerator());
            var mock3 = new Mock<ContextoDb>();
            mock3.Setup(o => o.Entrada).Returns(mockEntrada.Object);

            entradaRepositorio = new EntradaRepositorio(mock3.Object);

            controller = new RegistroAsistenciaController(registroRepositorio,webHostMock.Object , promotorRepositorio, eventoRepositorio, entradaRepositorio);
        }

        //CONTROLADOR TEST

        [Test]
        public void ListaAsistentesTest() {
            var result = controller.ListaAsistentes(1) as ViewResult;
            Assert.IsInstanceOf<List<RegistroAsistencia>>(result.Model);
        }


        //REPOSITORIO TEST
        [Test]
        public void ListaAsistentesRepositorioTest()
        {
            var listaEntradas = new List<Entrada>()
            {
                 new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=1,
                    CantidadEntradas=4,
                    EventoId=1
                },
                new Entrada {
                    IdCantidad=1,
                    IdUser=1,
                    EntradaId=2,
                    CantidadEntradas=4,
                    EventoId=1
                },
                new Entrada {
                    IdCantidad=5,
                    IdUser=2,
                    EntradaId=3,
                    CantidadEntradas=3,
                    EventoId=2
                },
            };


            var result = registroRepositorio.ListarAsistentes(listaEntradas);
            Assert.That(1, Is.EqualTo(result[0].Id));
        }

        [Test]
        public void ObtenerRegistroTest() {
            var result = registroRepositorio.ObtenerRegistro(1);
            Assert.That(1, Is.EqualTo(result.Id));
        }

        [Test]
        public void ObtenerTodosRegistrosTest()
        {
            var result = registroRepositorio.registros();
            Assert.That(2, Is.EqualTo(result[1].Id));
        }
    }
}
