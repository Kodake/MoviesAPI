using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackEnd.DTOs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        public ApplicationDbContext _context { get; }
        public IMapper _mapper { get; }

        public PeliculasController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            Pelicula pelicula = await _context.Peliculas
                .Include(p => p.Generos.OrderByDescending(p => p.Nombre))
                .Include(p => p.SalasDeCine)
                    .ThenInclude(p => p.Cine)
                .Include(p => p.PeliculasActores.Where(p => p.Actor.FechaNacimiento.Value.Year >= 1980))
                    .ThenInclude(p => p.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            PeliculaDTO peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);
            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

            return peliculaDTO;
        }

        [HttpGet("with-projectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            PeliculaDTO peliculaDTO = await _context.Peliculas
                .ProjectTo<PeliculaDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (peliculaDTO == null)
            {
                return NotFound();
            }

            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

            return peliculaDTO;
        }


        [HttpGet("cargado-selectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await _context.Peliculas
                .Select(p => new
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Generos = p.Generos.OrderByDescending(p => p.Nombre).Select(p => p.Nombre).ToList(),
                    CantidadActores = p.PeliculasActores.Count(),
                    CantidadCines = p.SalasDeCine.Select(p => p.CineId).Distinct().Count()
                }).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }

        [HttpGet("cargado-explicito/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
        {
            Pelicula pelicula = await _context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);

            int cantidadGeneros = await _context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if (pelicula == null)
            {
                return NotFound();
            }

            PeliculaDTO peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);

            return peliculaDTO;
        }

        [HttpGet("agrupado-carteleras")]
        public async Task<ActionResult<PeliculaDTO>> GetAgrupadasPorCarteleras()
        {
            var peliculasAgrupadas = await _context.Peliculas.GroupBy(p => p.EnCartelera)
                                        .Select(p => new
                                        {
                                            EnCartelera = p.Key,
                                            Conteo = p.Count(),
                                            Peliculas = p.ToList()
                                        }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }

        [HttpGet("agrupado-generos")]
        public async Task<ActionResult> GetAgrupadasPorGeneros()
        {
            var peliculasAgrupadas = await _context.Peliculas.GroupBy(p => p.Generos.Count())
                                        .Select(p => new
                                        {
                                            Conteo = p.Key,
                                            Titulos = p.Select(x => x.Titulo),
                                            Generos = p.Select(x => x.Generos).SelectMany(gen => gen).Select(gen => gen.Nombre).Distinct()
                                        }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltroDTO filtro)
        {
            IQueryable<Pelicula> peliculasQueryable = _context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(filtro.Titulo));
            }

            if (filtro.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
            }

            if (filtro.ProximosEstrenos)
            {
                DateTime hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
            }

            if (filtro.GeneroId is not 0)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Generos
                                                       .Select(g => g.Identificador)
                                                       .Contains(filtro.GeneroId));
            }

            List<Pelicula> peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();

            return _mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PeliculaCreacionDTO peliculaDTO)
        {
            Pelicula pelicula = _mapper.Map<Pelicula>(peliculaDTO);
            pelicula.Generos.ForEach(p => _context.Entry(p).State = EntityState.Unchanged);
            pelicula.SalasDeCine.ForEach(p => _context.Entry(p).State = EntityState.Unchanged);

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }

            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
