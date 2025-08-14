using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Pago
{
    public int Id { get; set; }

    public string? Cuenta { get; set; }

    public decimal? Monto { get; set; }

    public string? MetodoDePago { get; set; }

    public string? Nota { get; set; }

    public int FacturaId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
