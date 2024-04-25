using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class User
{
    public int Id { get; set; }

    public string Nick { get; set; } = null!;

    public string Hashpassword { get; set; } = null!;

    public bool Isadmin { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
