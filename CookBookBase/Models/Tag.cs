using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Tagname { get; set; } = null!;

    public virtual ICollection<Recipetotag> Recipetotags { get; set; } = new List<Recipetotag>();
}
