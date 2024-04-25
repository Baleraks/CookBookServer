using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Recipetoqauntity
{
    public int RtoiId { get; set; }

    public int Id { get; set; }

    public int ItoqId { get; set; }

    public virtual Ingridienttoqauntity Itoq { get; set; } = null!;

    public virtual Recipetoingridient Rtoi { get; set; } = null!;
}
