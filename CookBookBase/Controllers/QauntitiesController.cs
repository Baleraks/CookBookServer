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
    public class QauntitiesController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public QauntitiesController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Qauntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Qauntity>>> GetQauntitys()
        {
            return await _context.Qauntitys.ToListAsync();
        }

        // GET: api/Qauntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Qauntity>> GetQauntity(int id)
        {
            var qauntity = await _context.Qauntitys.FindAsync(id);

            if (qauntity == null)
            {
                return NotFound();
            }

            return qauntity;
        }

        // PUT: api/Qauntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQauntity(int id, Qauntity qauntity)
        {
            if (id != qauntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(qauntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QauntityExists(id))
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

        // POST: api/Qauntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Qauntity>> PostQauntity(Qauntity qauntity)
        {
            _context.Qauntitys.Add(qauntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQauntity", new { id = qauntity.Id }, qauntity);
        }

        // DELETE: api/Qauntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQauntity(int id)
        {
            var qauntity = await _context.Qauntitys.FindAsync(id);
            if (qauntity == null)
            {
                return NotFound();
            }

            _context.Qauntitys.Remove(qauntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QauntityExists(int id)
        {
            return _context.Qauntitys.Any(e => e.Id == id);
        }
    }
}
