using System;
using System.Collections.Generic;

namespace CookBookBase.Models;

public partial class Comment
{
    public string Commenttext { get; set; } = null!;

    public int Id { get; set; }

    public int UseId { get; set; }

    public int? RecId { get; set; }

    public int? Firstcommentid { get; set; }

    public virtual Comment? Firstcomment { get; set; }

    public virtual ICollection<Comment> InverseFirstcomment { get; set; } = new List<Comment>();

    public virtual Recipe? Rec { get; set; }

    public virtual User Use { get; set; } = null!;
}
