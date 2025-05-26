using API_SOS_Code.Data;
using API_SOS_Code.DTOs.Json;
using API_SOS_Code.Models;
using API_SOS_Code.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace API_SOS_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JsonController(AppDbContext context) { _context = context; }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> GetJson(IFormFile jsonFile)
        {
            if (jsonFile == null || jsonFile.Length == 0)
                return BadRequest("No file uploaded.");

            string json;
            using (var stream = jsonFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                json = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(json)) return NotFound("No data found!");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var packages = JsonSerializer.Deserialize<List<JsonPackage>>(json, options);
                if (packages == null) return BadRequest("Invalid JSON format.");

                var ingredients = new List<Ingredient>();
                var ingredientsName = _context.IngredientsName.ToList();

                foreach (var dish in packages)
                {
                    if (!DateTime.TryParseExact(dish.Content.ExpirationDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expirationDate))
                        return BadRequest($"Invalid expiration date format in a package.");

                    int totalUnits = dish.Amount * dish.Content.Units;
                    for (int i = 0; i < totalUnits; i++)
                    {
                        ingredients.Add(new Ingredient
                        {
                            Name = dish.Content.Name,
                            ExpirationDate = expirationDate
                        });

                        if (ingredientsName.All(i => i.Name != dish.Content.Name))
                        {
                            var newIngredientName = new IngredientName
                            {
                                Name = dish.Content.Name
                            };
                            ingredientsName.Add(newIngredientName);
                            _context.IngredientsName.Add(newIngredientName);  
                        }
                    }
                }

                _context.Ingredients.AddRange(ingredients);
                 await _context.SaveChangesAsync();

                return Ok(JsonSerializer.Serialize(packages, options));
            }
            catch
            {
                return BadRequest("Error reading the file.");
            }
        }
    }
}
