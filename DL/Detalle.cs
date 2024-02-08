using System;
using System.Collections.Generic;

namespace DL;

public partial class Detalle
{
    public int IdDetalle { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
