using System;
using System.Collections.Generic;

namespace DL;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public int IdDetalle { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal? Total { get; set; }

    public virtual Detalle IdDetalleNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
