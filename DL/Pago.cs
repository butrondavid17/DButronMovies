using System;
using System.Collections.Generic;

namespace DL;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdVenta { get; set; }

    public string TipoPago { get; set; } = null!;

    public DateTime FechaPago { get; set; }

    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}
