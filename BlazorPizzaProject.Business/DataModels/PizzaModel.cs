using System.ComponentModel.DataAnnotations;

namespace BlazorPizzaProject.Business.DataModels
{
    public class PizzaModel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Image { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public double SmallSize { get; set; }
        [Required]
        public double MediumSize { get; set; }
        [Required]
        public double LargeSize { get; set; }
        [Required]
        public double ExtraLargeSize { get; set; }
    }
}
