using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string Email { get; set; } = null!;

    public string? Contrasena { get; set; }

    public string? Telefono { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
