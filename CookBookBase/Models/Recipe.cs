using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Recipe
{
    public string Recipename { get; set; } = null!;

    public decimal Reportsnum { get; set; }

    public decimal Likes { get; set; }

    public string Recipeimagepath { get; set; } = null!;

    public decimal Cokingtime { get; set; }

    public int Id { get; set; }

    public int? UseId { get; set; }

    public double Recipecalories { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();

    public virtual ICollection<Recipetoingridient> Recipetoingridients { get; set; } = new List<Recipetoingridient>();

    public virtual ICollection<Recipetotag> Recipetotags { get; set; } = new List<Recipetotag>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public virtual User? Use { get; set; }
}
