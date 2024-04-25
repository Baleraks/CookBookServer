using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBookBase;
using CookBookBase.Models;

namespace CookBookBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipetoingridientsController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public RecipetoingridientsController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipetoingridients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipetoingridient>>> GetRecipetoingridients()
        {
            return await _context.Recipetoingridients.ToListAsync();
        }

        // GET: api/Recipetoingridients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipetoingridient>> GetRecipetoingridient(int id)
        {
            var recipetoingridient = await _context.Recipetoingridients.FindAsync(id);

            if (recipetoingridient == null)
            {
                return NotFound();
            }

            return recipetoingridient;
        }

        // PUT: api/Recipetoingridients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipetoingridient(int id, Recipetoingridient recipetoingridient)
        {
            if (id != recipetoingridient.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipetoingridient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipetoingridientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipetoingridients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipetoingridient>> PostRecipetoingridient(Recipetoingridient recipetoingridient)
        {
            _context.Recipetoingridients.Add(recipetoingridient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipetoingridient", new { id = recipetoingridient.Id }, recipetoingridient);
        }

        // DELETE: api/Recipetoingridients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipetoingridient(int id)
        {
            var recipetoingridient = await _context.Recipetoingridients.FindAsync(id);
            if (recipetoingridient == null)
            {
                return NotFound();
            }

            _context.Recipetoingridients.Remove(recipetoingridient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipetoingridientExists(int id)
        {
            return _context.Recipetoingridients.Any(e => e.Id == id);
        }
    }
}
