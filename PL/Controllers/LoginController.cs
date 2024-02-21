using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using ML;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LoginController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string pswd)
        {
            Dictionary<string, object> objeto = BL.Usuario.GetUsuarioByEmail(Email);
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                ML.Usuario usuario = (ML.Usuario)objeto["Usuario"];
                if (usuario.Email != "")
                {
                    var password = ConvertToBytes(pswd);
                    var validPassword = ByteArraysEqual(password, usuario.Password);
                    if (validPassword)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Mensaje = "La contraseña ingresada es incorrecta";
                        ViewBag.Password = false;

                    }
                }
                else
                {
                    ViewBag.Mensaje = "El correo es incorrecto";
                    ViewBag.Email = false;

                }
            }
            else
            {
                ViewBag.Mensaje = "El correo no se encontro";
                ViewBag.Email = false;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(ML.Usuario usuario, string password)
        {
            usuario.Password = ConvertToBytes(password);
            Dictionary<string, object> objeto = BL.Usuario.Add(usuario);
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mensaje = "Ha ocurrido un error al registrar el usuario";
            }
            return View();
        }
        [HttpGet]
        public ActionResult RecoveryPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecoveryPassword(string Email)
        {
            Dictionary<string, object> objeto = BL.Usuario.GetUsuarioByEmail(Email);
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                EnviarEmail(Email);
                ViewBag.Mensaje = "El correo de recuperacion ha sido eviado";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Mensaje = "El correo no esta ligado a una cuenta existente";
                ViewBag.Email = false;
                return View();
            }
        }
        [HttpGet]
        public ActionResult NewPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewPassword(string Password)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                ViewBag.Correct = true;
                var session = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(HttpContext.Session.GetString("Email"));
                Dictionary<string, object> objeto = BL.Usuario.GetUsuarioByEmail(session.Email);
                bool resultado = (bool)objeto["Resultado"];
                var password = ConvertToBytes(Password);
                if (resultado)
                {
                    Dictionary<string, object> action = BL.Usuario.UpdatePassword(session.Email, password);
                    bool resultadoPassword = (bool)action["Resultado"];
                    if (resultadoPassword)
                    {
                        ViewBag.Mensaje = "La contraseña ha sido actualizada correctamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        string excepcion = (string)action["Excepcion"];
                        ViewBag.Mensaje = "Ocurrio un error al intentar actualizar la contraseña " + excepcion;
                        return PartialView("Modal");
                    }
                }
                else
                {
                    string excepcion = (string)objeto["Excepcion"];
                    ViewBag.Mensaje = "Ocurrio un error " + excepcion;
                    return PartialView("Modal");
                }
            }
            {
                ViewBag.Correct = false;
                ViewBag.Mensaje = "El link de recuperación ha caducado";
                return View();
            }
            
        }
        public ActionResult EnviarEmail(string email)
        {
            //llamar al metodo
            string emailOrigen = "dabicho803@gmail.com";

            MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
            mailMessage.IsBodyHtml = true;

            //string contenidoHTML = System.IO.File.ReadAllText(@"C:\users\digis\Documents\IISExpress\Leonardo Escogido Bravo\Proyecto2023Ecommerce\PL\Views\Usuario\Email.html");

            //string contenidoHTML = System.IO.File.ReadAllText(Path.Combine("Views", "Usuario", "Email.html"));

            //mandemos a llamar a nuestro template
            string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));
            //sin template
            //string contenidoHTML = "<table> etc";

            mailMessage.Body = contenidoHTML;
            var emailEncrypt = Base64Encode(email);
            Dictionary<string, object> objeto = BL.Usuario.GetUsuarioByEmail(email);
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                ML.Usuario usuario = (ML.Usuario)objeto["Usuario"];
                string nombre = usuario.Nombre;
                string url = "http://localhost:5183/Login/NewPassword/" + HttpUtility.UrlEncode(emailEncrypt);
                mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
                mailMessage.Body = mailMessage.Body.Replace("{Name}", nombre);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "gknursejguovghwy ");
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
                HttpContext.Session.SetString("Email", Newtonsoft.Json.JsonConvert.SerializeObject(usuario));
                ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
                return PartialView("Modal");
            }
            else
            {
                string excepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = "Ocurrio un error " + excepcion;
                return PartialView("Modal");
            }
        }
        private byte[] ConvertToBytes(string pswd)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] hashBytes = sha256.ComputeHash(encoding.GetBytes(pswd));
                byte[] truncatedHash = new byte[20];
                Array.Copy(hashBytes, truncatedHash, 20);
                return truncatedHash;
            }
        }
        private bool ByteArraysEqual(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        {
            return a1.SequenceEqual(a2);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
