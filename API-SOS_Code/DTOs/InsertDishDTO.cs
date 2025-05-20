namespace API_SOS_Code.DTOs
{
    public class InsertDishDTO
    {
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required string Description { get; set; }
        public List<string> IngredientsName { get; set; } = new List<string>();
    }
}
