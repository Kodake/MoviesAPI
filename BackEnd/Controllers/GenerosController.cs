using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenerosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            return await _context.Generos.AsNoTracking()
                //.Where(g => !g.EstaBorrado) commented because we added a filter with HasQueryFilter in GeneroConfig
                .OrderBy(g => g.Nombre).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {
            Genero genero = await _context.Generos
                .FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }

            return genero;
        }

        [HttpGet("primer")]
        public async Task<ActionResult<Genero>> Primer()
        {
            Genero genero = await _context.Generos
                .FirstOrDefaultAsync(g => g.Nombre.StartsWith("C"));

            if (genero == null)
            {
                return NotFound();
            }

            return genero;
        }

        [HttpGet("filtrar")]
        public async Task<IEnumerable<Genero>> Filtrar()
        {
            return await _context.Generos
                .Where(g => g.Nombre.StartsWith("C") || g.Nombre.StartsWith("A"))
                .ToListAsync();
        }

        [HttpGet("filtrar-nombre")]
        public async Task<IEnumerable<Genero>> FiltrarNombre(string nombre)
        {
            return await _context.Generos
                .Where(g => g.Nombre.Contains(nombre))
                .OrderByDescending(g => g.Nombre)
                .ToListAsync();
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<IEnumerable<Genero>>> GetPaginacion(int pagina = 1)
        {
            int cantidadRegistrosPorPagina = 2;
            List<Genero> generos = await _context.Generos
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina)
                .ToListAsync();

            if (generos == null)
            {
                return NotFound();
            }

            return Ok(generos);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Genero genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Género ageregado satisfactoriamente" });
        }

        [HttpPost("multiple")]
        public async Task<ActionResult> PostMultiple(Genero[] generos)
        {
            _context.AddRange(generos);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Género ageregado satisfactoriamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Genero genero = await _context.Generos.FirstOrDefaultAsync(p => p.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }

            _context.Remove(genero);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            Genero genero = await _context.Generos.AsTracking().FirstOrDefaultAsync(p => p.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }

            genero.EstaBorrado = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id:int}")]
        public async Task<ActionResult> Restaurar(int id)
        {
            Genero genero = await _context.Generos.AsTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }

            genero.EstaBorrado = false;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
