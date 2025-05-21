namespace API_SOS_Code.DTOs.Ingredients
{
    public class GetIngredientDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> DishesName { get; set; } = new List<string>();
    }
}
