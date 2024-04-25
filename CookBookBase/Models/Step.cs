using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Step
{
    public string? Stepimagepath { get; set; }

    public decimal Steptime { get; set; }

    public int Id { get; set; }

    public int RecId { get; set; }

    public string Steptext { get; set; } = null!;

    public virtual Recipe Rec { get; set; } = null!;
}
