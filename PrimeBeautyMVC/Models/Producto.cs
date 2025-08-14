using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? CantidadExistente { get; set; }

    public string? Categoria { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<FacturasDetalle> FacturasDetalles { get; set; } = new List<FacturasDetalle>();
}
