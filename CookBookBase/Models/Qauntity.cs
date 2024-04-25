using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Qauntity
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Ingridienttoqauntity> Ingridienttoqauntities { get; set; } = new List<Ingridienttoqauntity>();
}
