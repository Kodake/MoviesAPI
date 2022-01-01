using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinesController : ControllerBase
    {
        public ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CinesController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await _context.Cines.ProjectTo<CineDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("cercanos")]
        public async Task<ActionResult> Get(double latitud, double longitud)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var miUbicacion = geometryFactory.CreatePoint(new Coordinate(longitud, latitud));
            var distanciaMaximaEnMetros = 2000;

            var cines = await _context.Cines
                .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                .Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaximaEnMetros))
                .Select(c => new
                {
                    Nombre = c.Nombre,
                    Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
                }).ToListAsync();

            return Ok(cines);
        }
    }
}
