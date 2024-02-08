using System;
using System.Collections.Generic;

namespace DL;

public partial class ProductoProveedor
{
    public int IdProducto { get; set; }

    public int IdProveedor { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
