using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Report
{
    public int RecId { get; set; }

    public int UseId { get; set; }

    public int Id { get; set; }

    public virtual Recipe Rec { get; set; } = null!;

    public virtual User Use { get; set; } = null!;
}
