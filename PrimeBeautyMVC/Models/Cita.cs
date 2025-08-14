using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Cita
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int EmpleadoId { get; set; }

    public DateTime? Fecha { get; set; }

    public TimeSpan? Hora { get; set; }

    public string? Estado { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Usuario Usuario { get; set; } = null!;
}
