using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll(int? IdCategoria)
        {
            if (IdCategoria != null)
            {
                ML.Producto producto = new ML.Producto();
                Dictionary<string, object> objeto = BL.Producto.GetByIdCategoria(IdCategoria.Value);
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
            else
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
        }
        [HttpGet]
        public IActionResult Form(int IdProducto)
        {
            ML.Producto producto = new ML.Producto();
            Dictionary<string, object> objeto = BL.Producto.GetById(IdProducto);
            bool result = (bool)objeto["Resultado"];
            if (result)
            {
                producto = (ML.Producto)objeto["Producto"];
                return View(producto);
            }
            else
            {
                string excepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = "Ocurrio un error al recuperar la información" + excepcion;
                return PartialView("Modal");
            }
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
                string exepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = "La imagen no se pudo actualizar " + exepcion;
                return PartialView("Modal");
            }
        }
        [HttpGet]
        public IActionResult AddProducto(int IdProducto)
        {
            Dictionary<string, object> objProducto = BL.Producto.GetById(IdProducto);
            ML.Producto producto = (ML.Producto)objProducto["Producto"];
            ML.Carrito carrito = new ML.Carrito();
            carrito.listaProductos = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                producto.Cantidad = 1;
                carrito.listaProductos.Add(producto);
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.listaProductos));
                ViewBag.Mensaje = $"Producto {producto.Nombre} agregado al carrito";
                return PartialView("Modal");
            }
            else
            {
                GetCarrito(carrito);
                bool productExist = false;
                foreach (ML.Producto product in carrito.listaProductos)
                {
                    if (IdProducto == product.IdProducto)
                    {
                        productExist = true;
                        product.Cantidad++;
                        break;
                    }
                    else
                    {
                        productExist = false;
                        producto.Cantidad+=1;
                    }
                }
                if (productExist)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.listaProductos));
                }
                else
                {
                    producto.Cantidad = 1;
                    carrito.listaProductos.Add(producto);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.listaProductos));
                }
                ViewBag.Mensaje = $"Producto {producto.Nombre} agregado al carrito";
                return PartialView("Modal");
            }
        }
        [HttpGet]
        public IActionResult Carrito()
        {
            string productos = HttpContext.Session.GetString("Carrito");
            ML.Carrito carrito = new ML.Carrito();
            if (productos == null)
            {
                return View(carrito);
            }
            else
            {
                var carritoSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));
                carrito.listaProductos = new List<object>();
                foreach (var obj in carritoSession)
                {
                    ML.Producto productosDeserializados = new ML.Producto();
                    productosDeserializados = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    carrito.listaProductos.Add(productosDeserializados);
                }
                return View(carrito);
            }
        }
        public ML.Carrito GetCarrito(ML.Carrito carrito)
        {
            var session = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));
            foreach (var obj in session)
            {
                ML.Producto objCarrito = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                carrito.listaProductos.Add(objCarrito);
            }
            return carrito;
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
