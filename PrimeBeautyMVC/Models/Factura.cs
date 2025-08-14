using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Factura
{
    public int Id { get; set; }

    public int CitaId { get; set; }

    public int UsuarioId { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Total { get; set; }

    public virtual Cita Cita { get; set; } = null!;

    public virtual ICollection<FacturasDetalle> FacturasDetalles { get; set; } = new List<FacturasDetalle>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Usuario Usuario { get; set; } = null!;
}
