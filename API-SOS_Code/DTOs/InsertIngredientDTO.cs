namespace API_SOS_Code.DTOs
{
    public class InsertIngredientDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
