using GAMMAFEST.Data;
using GAMMAFEST.Helpers;
using GAMMAFEST.Models;
using Microsoft.AspNetCore.Mvc;

namespace GAMMAFEST.Repositorio
{
    public interface IPromotorRepositorio
    {
        bool RegistrarUsuario(string email, string password, string nombre, string apellidos, string tipo);
        UserPromotor LogInUsuario(string email, string password);
        bool ExisteEmail(string email);
        UserPromotor ObtenerInicio(string username);
        int ConteoUser();
        UserPromotor ObtenerUserById(int? id);
    }
    public class PromotorRepositorio: IPromotorRepositorio
    {
        
        public readonly ContextoDb _context;

        public PromotorRepositorio(ContextoDb context)
        {
            _context = context;
        }

        public bool ExisteEmail(string email)
        {
            var consulta = from datos in this._context.UserPromotor
                           where datos.Email == email
                           select datos;
            if (consulta.Count() > 0)
            {
                //El email existe en la base de datos
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos, string tipo)
        {
            bool ExisteEmail = this.ExisteEmail(email);
            if (ExisteEmail)
            {
                return false;
            }
            else
            {
                //int idPromotor = this.GetMaxIdUsuario();
                UserPromotor userPromotor=new UserPromotor();
                //userPromotor.IdPromotor = idPromotor;
                userPromotor.Nombre = nombre;
                userPromotor.Apellido = apellidos;
                userPromotor.Email = email;
                userPromotor.tipoUsuario = tipo;
                //GENERAMOS UN SALT ALEATORIO PARA CADA USUARIO
                userPromotor.Cifrado = HelperCryptography.GenerateSalt();
                //GENERAMOS SU PASSWORD CON EL SALT
                userPromotor.Contrasenia = HelperCryptography.EncriptarPassword(password, userPromotor.Cifrado);
                _context.Add(userPromotor);
                _context.SaveChanges();

                return true;
            }

        }

        public UserPromotor LogInUsuario(string email, string password)
        {
            UserPromotor promotor = _context.UserPromotor.SingleOrDefault(x => x.Email == email);
            if (promotor == null)
            {
                return null;
            }
            else
            {
                //Debemos comparar con la base de datos el password haciendo de nuevo el cifrado con cada salt de usuario
                byte[] passUsuario = promotor.Contrasenia;
                string salt = promotor.Cifrado;
                //Ciframos de nuevo para comparar
                byte[] temporal = HelperCryptography.EncriptarPassword(password, salt);

                //Comparamos los arrays para comprobar si el cifrado es el mismo
                bool respuesta = HelperCryptography.compareArrays(passUsuario, temporal);
                if (respuesta == true)
                {
                    return promotor;
                }
                else
                {
                    //Contraseña incorrecta
                    return null;
                }
            }
        }

        public int ConteoUser() {
            return _context.UserPromotor.Count();
        }

        public UserPromotor ObtenerInicio(string username)
        {
            return _context.UserPromotor.FirstOrDefault(o => o.Nombre == username);
        }

        public UserPromotor ObtenerUserById(int? id)
        {
            return _context.UserPromotor.Single(u => u.IdUser == id);
        }


    }
}
