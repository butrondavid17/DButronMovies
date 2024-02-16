using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Update;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
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
    }
}
