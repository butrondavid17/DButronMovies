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
                                      Email = tablaUsuario.Email,
                                      Password = tablaUsuario.Password
                                  }).SingleOrDefault();
                    if (objeto != null)
                    {
                        usuario.Email = objeto.Email;
                        usuario.Password = objeto.Password;
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