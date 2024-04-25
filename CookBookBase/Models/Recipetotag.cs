using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Recipetotag
{
    public int RecId { get; set; }

    public int TagId { get; set; }

    public int Id { get; set; }

    public virtual Recipe Rec { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
