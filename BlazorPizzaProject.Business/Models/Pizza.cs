namespace BlazorPizzaProject.Business.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double SmallSize { get; set; }
        public double MediumSize { get; set; }
        public double LargeSize { get; set; }
        public double ExtraLargeSize { get; set; }
    }
}
