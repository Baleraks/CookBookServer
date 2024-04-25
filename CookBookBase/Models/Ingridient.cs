using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Ingridient
{
    public int Id { get; set; }

    public string Ingridienname { get; set; } = null!;

    public double Ingridientcalories { get; set; }

    public virtual ICollection<Ingridienttoqauntity> Ingridienttoqauntities { get; set; } = new List<Ingridienttoqauntity>();

    public virtual ICollection<Recipetoingridient> Recipetoingridients { get; set; } = new List<Recipetoingridient>();
}
