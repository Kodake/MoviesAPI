using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackEnd.DTOs;
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
    }
}
