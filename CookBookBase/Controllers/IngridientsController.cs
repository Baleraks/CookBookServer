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
    public class IngridientsController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public IngridientsController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Ingridients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingridient>>> GetIngridients()
        {
            return await _context.Ingridients.ToListAsync();
        }

        // GET: api/Ingridients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingridient>> GetIngridient(int id)
        {
            var ingridient = await _context.Ingridients.FindAsync(id);

            if (ingridient == null)
            {
                return NotFound();
            }

            return ingridient;
        }

        // PUT: api/Ingridients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngridient(int id, Ingridient ingridient)
        {
            if (id != ingridient.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingridient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngridientExists(id))
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

        // POST: api/Ingridients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingridient>> PostIngridient(Ingridient ingridient)
        {
            _context.Ingridients.Add(ingridient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngridient", new { id = ingridient.Id }, ingridient);
        }

        // DELETE: api/Ingridients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngridient(int id)
        {
            var ingridient = await _context.Ingridients.FindAsync(id);
            if (ingridient == null)
            {
                return NotFound();
            }

            _context.Ingridients.Remove(ingridient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngridientExists(int id)
        {
            return _context.Ingridients.Any(e => e.Id == id);
        }
    }
}
