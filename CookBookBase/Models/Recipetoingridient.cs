using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Recipetoingridient
{
    public int RecId { get; set; }

    public int IngId { get; set; }

    public int Id { get; set; }

    public virtual Ingridient Ing { get; set; } = null!;

    public virtual Recipe Rec { get; set; } = null!;

    public virtual ICollection<Recipetoqauntity> Recipetoqauntities { get; set; } = new List<Recipetoqauntity>();
}
