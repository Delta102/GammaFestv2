using System.Security.Cryptography;
using System.Text;

namespace GAMMAFEST.Helpers
{
    public static class HelperCryptography
    {
        public static string GenerateSalt()
        {
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int numero = RandomNumberGenerator.GetInt32(0, 255);
                char letra = Convert.ToChar(numero);
                salt += letra;
            }
            return salt;
        }

        public static bool compareArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                //comparamos byte a byte
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }

            return iguales;
        }

        public static byte[] EncriptarPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            for (int i = 1; i <= 107; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;

        }
    }
}
