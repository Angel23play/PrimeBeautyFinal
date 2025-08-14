using System;
using System.Collections.Generic;

namespace PrimeBeautyMVC.Models;

public partial class Empleado
{
    public int Id { get; set; }

    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }

    public string? Tipo { get; set; }

    public string? Telefono { get; set; }


    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
