using API_SOS_Code.Data;
using API_SOS_Code.DTOs.Dishes;
using API_SOS_Code.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_SOS_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DishController(AppDbContext context) { _context = context; }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAll()
        {
            var dishes = await _context.Dishes.ToListAsync();
            if (dishes.Count == 0) return NotFound("No dishes found!");
            return Ok(dishes);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetById(int id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null) return NotFound($"Dish {id} not found!");
            return Ok(dish);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Dish>> Add(InsertDishDTO insertDishDTO)
        {
            try
            {
                if (insertDishDTO == null) return BadRequest("Dish data is required!");

                var dish = new Dish
                {
                    Name = insertDishDTO.Name,
                    ImageUrl = insertDishDTO.ImageUrl,
                    Description = insertDishDTO.Description,
                    Price = insertDishDTO.Price,
                    IngredientsName = insertDishDTO.IngredientsName
                };

                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = dish.Id } , dish);
            }
            catch (DbUpdateException)
            {
                return BadRequest("An error occurred while adding the dish. Try again.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dish>> Delete(int id)
        {
            try
            {
                var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
                
                if (dish == null) return NotFound($"Dish {id} not found!");

                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
                return Ok(dish);
            }
            catch (DbUpdateException)
            {
                return BadRequest($"An error ocurred while deleting the dish with id {id}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Ingredient>> Update(int id, InsertDishDTO dishDTO)
        {
            try
            {
                var dish = await _context.Dishes.FirstOrDefaultAsync(g => g.Id == id);

                if (dish == null) return NotFound($"Dish {id} not found!");

                dish.Name = dishDTO.Name;
                dish.ImageUrl = dishDTO.ImageUrl;
                dish.Description = dishDTO.Description;
                dish.Price = dishDTO.Price;
                dish.IngredientsName = dishDTO.IngredientsName;

                await _context.SaveChangesAsync();
                return Ok(dish);
            }
            catch (DbUpdateException)
            {
                return BadRequest("An error ocurred while updating the ingredient. Try again.");
            }
        }
    }
}
