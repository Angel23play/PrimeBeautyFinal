using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class FacturasDetalle
{
    public int Id { get; set; }

    public int FacturaId { get; set; }

    public int? ProductoId { get; set; }

    public int? ServicioId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Total { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Producto? Producto { get; set; }

    public virtual Servicio? Servicio { get; set; }
}
