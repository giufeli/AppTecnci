using AppTecnici.Server.Data;
using AppTecnici.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTecnici.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpiantiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImpiantiController(AppDbContext context)
        {
            _context = context;
        }

        // GET:
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Impianto>>> GetImpianti()
        {
            return await _context.Impianti.ToListAsync();
        }

        // POST:
        [HttpPost]
        public async Task<ActionResult<Impianto>> PostImpianto(Impianto impianto)
        {
            _context.Impianti.Add(impianto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetImpianti), new { id = impianto.Id }, impianto);
        }

        // DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImpianto(int id)
        {
            var impianto = await _context.Impianti.FindAsync(id);
            if (impianto == null) return NotFound();

            _context.Impianti.Remove(impianto);
            await _context.SaveChangesAsync();

            // Se la tabella è vuota, resetta l'ID a 0 (così il prossimo sarà 1)
            if (!await _context.Impianti.AnyAsync())
            {
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Impianti', RESEED, 0)");
            }

            return NoContent();
        }

        // PUT:
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImpianto(int id, Impianto impianto)
        {
            if (id != impianto.Id)
            {
                return BadRequest("L'ID dell'URL non corrisponde all'ID dell'oggetto.");
            }

            _context.Entry(impianto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Impianti.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // HTTP 204: Modifica avvenuta con successo
        }
    }
}