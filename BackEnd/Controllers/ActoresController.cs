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
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Get()
        {
            List<ActorDTO> actores = await _context.Actores.Select(a => new ActorDTO { Id = a.Id, Nombre = a.Nombre }).ToListAsync();

            if (actores is null)
            {
                return NotFound();
            }

            return Ok(actores);
        }

        [HttpGet("with-mapper")]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetMapper()
        {
            List<ActorDTO> actores = await _context.Actores
                .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (actores is null)
            {
                return NotFound();
            }

            return Ok(actores);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO actorDTO)
        {
            Actor actor = _mapper.Map<Actor>(actorDTO);
            _context.Add(actor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreacionDTO actorDTO, int id)
        {
            Actor actorDB = await _context.Actores.AsTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = _mapper.Map(actorDTO, actorDB);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("desconectado/{id:int}")]
        public async Task<ActionResult> PutDesconectado(ActorCreacionDTO actorDTO, int id)
        {
            bool existeActor = await _context.Actores.AnyAsync(p => p.Id == id);

            if (!existeActor)
            {
                return NotFound();
            }

            Actor actor = _mapper.Map<Actor>(actorDTO);
            actor.Id = id;

            _context.Update(actor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
