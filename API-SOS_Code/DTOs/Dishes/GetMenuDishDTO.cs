using API_SOS_Code.Models;

namespace API_SOS_Code.DTOs.Dishes
{
    public class GetMenuDishDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public double Price { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
