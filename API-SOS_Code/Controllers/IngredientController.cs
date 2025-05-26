using API_SOS_Code.Data;
using API_SOS_Code.DTOs.Ingredients;
using API_SOS_Code.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_SOS_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientController(AppDbContext context) { _context = context; }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetAll()
        {
            var ingredients = await _context.Ingredients
                .ToListAsync();

            if (ingredients.Count == 0) return NotFound("No ingredients found!");

            return Ok(ingredients);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetById(int id)
        {
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null) return NotFound($"Ingredient {id} not found!");

            return Ok(ingredient);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Ingredient>> Add(InsertIngredientDTO ingredientDTO)
        {
 
            if (ingredientDTO == null) return BadRequest("Ingredient data is required!");

            var ingredients = new List<Ingredient>();
            for (int i = 0; i < ingredientDTO.Quantity; i++)
            {
                ingredients.Add(new Ingredient
                {
                    Name = ingredientDTO.Name,
                    ExpirationDate = ingredientDTO.ExpirationDate
                });
            }

            await _context.Ingredients.AddRangeAsync(ingredients);

            // Check if the ingredient name already exists
            var ingredientsName = await _context.IngredientsName.ToListAsync();
            foreach (var ingredient in ingredients)
            {
                if (ingredientsName.All(i => i.Name != ingredient.Name))
                {
                    var newIngredientName = new IngredientName
                    {
                        Name = ingredient.Name
                    };
                    ingredientsName.Add(newIngredientName);
                    await _context.IngredientsName.AddAsync(newIngredientName);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAll), ingredients);
            }
            catch (DbUpdateException)
            {
                return BadRequest("An error occurred while adding the ingredient. Try again.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ingredient>> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ingredient ID!");

            try
            {
                var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);

                if (ingredient == null) return NotFound($"Ingredient {id} not found!");

                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                return Ok(ingredient);
            }
            catch (DbUpdateException)
            {
                return BadRequest($"An error ocurred while deleting the ingredient with id {id}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Ingredient>>> DeleteBatch([FromBody] IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any()) return BadRequest("No ingredient IDs provided for deletion!");

            try
            {
                var ingredients = new List<Ingredient>();

                foreach (var ingredientId in ids)
                {
                    var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == ingredientId);
                    if (ingredient != null)
                    {
                        ingredients.Add(ingredient);
                    }
                }

                ingredients.ForEach(ingredient => _context.Ingredients.Remove(ingredient));

                await _context.SaveChangesAsync();
                return Ok(ingredients);
            }
            catch (DbUpdateException)
            {
                return BadRequest($"An error ocurred while deleting the ingredient with id {ids}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Ingredient>> Update(int id, UpdateIngredientDTO ingredientDTO)
        {
            try
            {
                var ingredient = await _context.Ingredients.FirstOrDefaultAsync(g => g.Id == id);

                if (ingredient == null) return NotFound($"Ingredient {id} not found!");

                ingredient.Name = ingredientDTO.Name;
                ingredient.ExpirationDate = ingredientDTO.ExpirationDate;

                await _context.SaveChangesAsync();
                return Ok(ingredient);
            }
            catch (DbUpdateException)
            {
                return BadRequest("An error ocurred while updating the ingredient. Try again.");
            }
        }
    }
}
