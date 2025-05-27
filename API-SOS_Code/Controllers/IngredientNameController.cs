using API_SOS_Code.Data;
using API_SOS_Code.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_SOS_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientNameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientNameController(AppDbContext context) { _context = context; }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAll()
        {
            List<string> ingredients = await _context.IngredientsName
                .Select(i => i.Name)
                .ToListAsync();

            if (ingredients.Count == 0) return NotFound("No ingredients name found!");

            return Ok(ingredients);
        }
    }
}
