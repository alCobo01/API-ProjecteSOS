﻿namespace API_SOS_Code.DTOs.Ingredients
{
    public class InsertIngredientDTO
    {
        public required string Name { get; set; }
        public int Quantity { get; set; }   
        public DateTime ExpirationDate { get; set; }
    }
}
