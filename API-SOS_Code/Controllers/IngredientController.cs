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
            try
            {
                if (ingredientDTO == null) return BadRequest("Ingredient data is required!");

                var ingredient = new Ingredient
                {
                    Name = ingredientDTO.Name,
                    ExpirationDate = ingredientDTO.ExpirationDate
                };

                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = ingredient.Id } , ingredient);
            }
            catch (DbUpdateException)
            {
                return BadRequest("An error occurred while adding the ingredient. Try again.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<Ingredient>> Delete(int id)
        {
            try
            {
                var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
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
        [HttpPut("{id}")]
        public async Task<ActionResult<Ingredient>> Update(int id, InsertIngredientDTO ingredientDTO)
        {
            try
            {
                var ingredient = await _context.Ingredients.FirstOrDefaultAsync(g => g.Id == id);

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
