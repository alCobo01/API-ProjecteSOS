using API_SOS_Code.Data;
using API_SOS_Code.DTOs.Dishes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_SOS_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MenuController(AppDbContext context) => _context = context;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMenuDishDTO>>> Get()
        {
            var dishes = await _context.Dishes.ToListAsync();
            var ingredients = await _context.Ingredients.ToListAsync();

            var menu = dishes.Select(d => new GetMenuDishDTO
            {
                Id = d.Id,
                Name = d.Name,
                ImageUrl = d.ImageUrl,
                Description = d.Description,
                Price = d.Price,
                Ingredients = d.IngredientsName
                    .Select(ingredientName =>
                        ingredients
                            .Where(i => i.Name == ingredientName)
                            .OrderBy(i => i.ExpirationDate)
                            .FirstOrDefault()
                    )
                    .Where(i => i != null)
                    .ToList()!
            }).ToList();

            if (menu.Count == 0) return NotFound("No dishes found!");
            return Ok(menu);
        }
    }
}
