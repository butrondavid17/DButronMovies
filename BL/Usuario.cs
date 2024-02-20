using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {
        public static Dictionary<string, object> Add(ML.Usuario usuario)
        {
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> {{ "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Username}', '{usuario.Email}', @Password", new SqlParameter("@Password", System.Data.SqlDbType.VarBinary) { Value = usuario.Password } );           
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                        diccionario["Usuario"] = usuario;
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
        public static Dictionary<string, object> UpdatePassword(string email, byte[] password)
        {
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { {"Excepcion", excepcion}, { "Resultado", true} };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"UpdatePassword '{email}', @Password", new SqlParameter("@Password", System.Data.SqlDbType.VarBinary) { Value = password});
                    if (filasAfectadas >0)
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
        public static Dictionary<string, object> GetUsuarioByEmail(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Usuario", usuario }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronMovieContext context = new DL.DbutronMovieContext())
                {
                    var objeto = (from tablaUsuario in context.Usuarios
                                  where tablaUsuario.Email == email
                                  select new
                                  {
                                      IdUsuario = tablaUsuario.IdUsuario,
                                      Email = tablaUsuario.Email,
                                      Password = tablaUsuario.Password,
                                      Nombre = tablaUsuario.Nombre
                                  }).SingleOrDefault();
                    if (objeto != null)
                    {
                        usuario.IdUsuario = objeto.IdUsuario;
                        usuario.Email = objeto.Email;
                        usuario.Password = objeto.Password;
                        usuario.Nombre = objeto.Nombre;
                        diccionario["Resultado"] = true;
                        diccionario["Usuario"] = usuario;
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