using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Carrito
    {
        public ML.Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }  
        public List<object> listaProductos { get; set; }
    }
}
