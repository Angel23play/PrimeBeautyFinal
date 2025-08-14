using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Servicio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<FacturasDetalle> FacturasDetalles { get; set; } = new List<FacturasDetalle>();
}
