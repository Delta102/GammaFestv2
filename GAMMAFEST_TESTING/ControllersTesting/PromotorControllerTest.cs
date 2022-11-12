using GAMMAFEST.Controllers;
using GAMMAFEST.Data;
using GAMMAFEST.Helpers;
using GAMMAFEST.Models;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SixLabors.ImageSharp;

namespace GAMMAFEST_TESTING.ControllersTesting
{
    public class PromotorControllerTest
    {
        PromotorController controller;
        PromotorRepositorio eRepositorio;

        IQueryable lista;
        string password = "TestPass";
        string testPrueba;
        byte[] pass;

        [SetUp]
        public void Setup() {
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

            eRepositorio = new PromotorRepositorio(mock.Object);

            controller = new PromotorController(eRepositorio);
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
            var controller = new PromotorController(eRepositorio);
            var result = controller.Login("Usertest@user.pe", password + "asd");
            Assert.That(controller.ViewData["MENSAJE"], Is.EqualTo("No tienes credenciales correctas"));
            Assert.IsInstanceOf<Task<IActionResult>>(result);
        }

        [Test]
        public void LoginUserNullTest()
        {
            var controller = new PromotorController(eRepositorio);
            var result = controller.Login("Usertest6@user6.pe", password + "asd");
            Assert.That(controller.ViewData["MENSAJE"], Is.EqualTo("No tienes credenciales correctas"));
            Assert.IsInstanceOf<Task<IActionResult>>(result);
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