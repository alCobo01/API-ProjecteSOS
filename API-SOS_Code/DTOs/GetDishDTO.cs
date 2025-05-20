namespace API_SOS_Code.DTOs
{
    public class GetDishDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public List<string> IngredientsName { get; set; } = new List<string>();
    }
}
