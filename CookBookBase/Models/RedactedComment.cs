namespace CookBookBase.Models
{
    public class RedactedComment
    {
        public string Commenttext { get; set; } = null!;

        public int UseId { get; set; }

        public int? RecId { get; set; }

        public int? Firstcommentid { get; set; }
    }
}
