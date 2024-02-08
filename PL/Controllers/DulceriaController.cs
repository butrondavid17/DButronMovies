﻿using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            Dictionary<string, object> objeto = BL.Producto.GetAll();
            bool result = (bool)objeto["Resultado"];
            if (result)
            {
                producto = (ML.Producto)objeto["Producto"];
                return View(producto);
            }
            else
            {
                string excepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = "Ocurrio un error " + excepcion;
                return PartialView("Modal");
            }
        }
        [HttpGet]
        public IActionResult Form(int IdProducto)
        {
            Dictionary<string, object> objeto = BL.Producto.GetById(IdProducto);
            bool result = (bool)objeto["Resultado"];

            return View();
        }
        [HttpPost]
        public IActionResult Form(ML.Producto producto, IFormFile inImagen)
        {
            if (inImagen != null && inImagen.Length > 0)
            {
                producto.Imagen = ConvertToBytes(inImagen);
            }
            Dictionary<string, object> objeto = BL.Producto.UpdateImage(producto);
            bool result = (bool)objeto["Resultado"];
            if (result == true)
            {
                ViewBag.Mensaje = "La imagen ha sido actualizada";
                return PartialView("Modal");
            }
            else
            {
                string exepcion = (string)objeto["Exepcion"];
                ViewBag.Mensaje = "La imagen no se pudo actualizar " + exepcion;
                return PartialView("Modal");
            }
        }
        private byte[] ConvertToBytes(IFormFile file)
        {
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }
            return data;
        }
    }
}