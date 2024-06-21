namespace CookBookBase.Helpers.DataHelpers
{
    public class RedactedRecipe
    {
        public string Recipename { get; set; } = null!;
        public double Recipecalories { get; set; }
        public string[] StepText { get; set; } = null!;
        public byte[] MainImage { get; set; } = null!;
        public byte[][] StepImages { get; set; } = null!;
        public int? UserId { get; set; }
        public string[] Tags { get; set; } = null!;
        public double[] IngridientCalories { get; set; } = null!;
        public string[] Ingridients { get; set; } = null!;
        public string[] Qauntities { get; set; } = null!;

    }
}
