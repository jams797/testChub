using System.Globalization;
using System.Text.RegularExpressions;

namespace Consultorio_Seguros.Helper
{
    public class ValidationAsegurado
    {

        /// <summary>
        /// Validacion de nombre
        /// </summary>
        public bool EsNombreValido(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre) && nombre.Length <= 100;
        }

        /// <summary>
        /// Verifica si la cedula es valida
        /// </summary>
        public bool ValidarCedula(string cedula)
        {
            int suma = 0;
            int[] coeficientes = new int[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

            if (cedula.Length != 10)
            {
                return false;
            }

            int provincia = int.Parse(cedula.Substring(0, 2));
            if (provincia < 1 || provincia > 24)
            {
                return false;
            }

            int tercerDigito = int.Parse(cedula[2].ToString());
            if (tercerDigito < 0 || tercerDigito > 6)
            {
                return false;
            }

            for (int i = 0; i < coeficientes.Length; i++)
            {
                int valor = int.Parse(cedula[i].ToString()) * coeficientes[i];
                suma += valor > 9 ? valor - 9 : valor;
            }

            int decenaSuperior = (suma % 10 == 0) ? suma : ((suma / 10) + 1) * 10;
            int digitoVerificador = decenaSuperior - suma;

            if (digitoVerificador != int.Parse(cedula[9].ToString()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifica si son solo numeros el parametro recibido
        /// </summary>
        public bool IsDigitOnly(string numberPhone)
        {
            return numberPhone.All(char.IsDigit);
        }

        /// <summary>
        /// Verifica si el correo recibido por parametro es valido
        /// </summary>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
