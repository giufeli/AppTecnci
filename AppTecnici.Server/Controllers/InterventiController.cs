using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppTecnici.Shared.Models;
using AppTecnici.Server.Data;

namespace AppTecnici.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InterventiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/interventi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intervento>>> GetInterventi()
        {
            return await _context.Interventi.ToListAsync();
        }

        // POST: api/interventi
        [HttpPost]
        public async Task<ActionResult<Intervento>> PostIntervento(Intervento intervento)
        {
            _context.Interventi.Add(intervento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInterventi), new { id = intervento.Id }, intervento);
        }

        // DELETE: api/interventi
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntervento(int id)
        {
            var intervento = await _context.Interventi.FindAsync(id);
            if (intervento == null) return NotFound();

            _context.Interventi.Remove(intervento);
            await _context.SaveChangesAsync();

            // Se la tabella è vuota, resetta l'ID a 0 (così il prossimo sarà 1)
            if (!await _context.Interventi.AnyAsync())
            {
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Interventi', RESEED, 0)");
            }

            return NoContent();
        }

        // PUT: api/interventi
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntervento(int id, Intervento intervento)
        {
            if (id != intervento.Id)
            {
                return BadRequest("L'ID dell'URL non corrisponde all'ID dell'oggetto.");
            }

            _context.Entry(intervento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Interventi.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // HTTP 204
        }
    }
}