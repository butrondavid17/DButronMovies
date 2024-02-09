using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static Dictionary<string, object> GetByIdCategoria(int idCategoria)
        {
            ML.Producto producto = new ML.Producto();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Producto", producto}, {"Excepcion", excepcion}, { "Resultado", false} };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    var listaProductos = (from productos in context.Productos
                                          join categorias in context.Categoria on productos.IdCategoria equals categorias.IdCategoria
                                          where productos.IdCategoria == idCategoria
                                          select new                                          
                                          {
                                              IdProducto = productos.IdProducto,
                                              Nombre = productos.Nombre,
                                              Precio = productos.Precio,
                                              Imagen = productos.Imagen,
                                              IdCategoria = categorias.IdCategoria,
                                              NombreCategoria = categorias.Nombre
                                          }).ToList();
                    if (listaProductos.Count > 0)
                    {
                        producto.Productos = new List<object>();
                        foreach (var registro in listaProductos)
                        {
                            ML.Producto product = new ML.Producto();
                            product.IdProducto = registro.IdProducto;
                            product.Nombre = registro.Nombre;
                            product.Precio = registro.Precio;
                            product.Imagen = registro.Imagen;
                            product.Categoria = new ML.Categoria();
                            product.Categoria.IdCategoria = registro.IdCategoria;
                            product.Categoria.Nombre = registro.NombreCategoria;
                            producto.Productos.Add(product);
                        }
                        diccionario["Resultado"] = true;
                        diccionario["Producto"] = producto;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> GetAll()
        {
            string excepcion = "";
            ML.Producto producto = new ML.Producto();
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Producto", producto }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    var listaProductos = (from productos in context.Productos
                                          join categorias in context.Categoria on productos.IdCategoria equals categorias.IdCategoria
                                          select new
                                          {
                                              IdProducto = productos.IdProducto,
                                              Nombre = productos.Nombre,
                                              Precio = productos.Precio,
                                              Imagen = productos.Imagen,
                                              IdCategoria = categorias.IdCategoria,
                                              NombreCategoria = categorias.Nombre
                                          }).ToList();
                    if (listaProductos.Count > 0)
                    {
                        producto.Productos = new List<object>();
                        foreach (var registro in listaProductos)
                        {
                            ML.Producto product = new ML.Producto();
                            product.IdProducto = registro.IdProducto;
                            product.Nombre = registro.Nombre;
                            product.Precio = registro.Precio;
                            product.Imagen = registro.Imagen;
                            product.Categoria = new ML.Categoria();
                            product.Categoria.IdCategoria = registro.IdCategoria;
                            product.Categoria.Nombre = registro.NombreCategoria;
                            producto.Productos.Add(product);
                        }
                        diccionario["Resultado"] = true;
                        diccionario["Producto"] = producto;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> GetById(int idProducto)
        {
            string excepcion = "";
            ML.Producto producto = new ML.Producto();
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Producto", producto }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    var listaProductos = (from productos in context.Productos
                                          join categorias in context.Categoria on productos.IdCategoria equals categorias.IdCategoria
                                          where productos.IdProducto == idProducto
                                          select new
                                          {
                                              IdProducto = productos.IdProducto,
                                              Nombre = productos.Nombre,
                                              Precio = productos.Precio,
                                              Imagen = productos.Imagen,
                                              IdCategoria = productos.IdCategoria,
                                              NombreCategoria = categorias.Nombre
                                          }).FirstOrDefault();
                    if (listaProductos != null)
                    {
                        producto.IdProducto = listaProductos.IdProducto;
                        producto.Nombre = listaProductos.Nombre;
                        producto.Precio = listaProductos.Precio;
                        producto.Imagen = listaProductos.Imagen;
                        producto.Categoria = new ML.Categoria();
                        producto.Categoria.IdCategoria = listaProductos.IdCategoria;
                        producto.Categoria.Nombre = listaProductos.NombreCategoria;
                        diccionario["Resultado"] = true;
                        diccionario["Producto"] = producto;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> UpdateImage(ML.Producto producto)
        {
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"ProductoUpdateImage {producto.IdProducto}, @Imagen", new SqlParameter("@Imagen", System.Data.SqlDbType.VarBinary) { Value = producto.Imagen });
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
    }
}
