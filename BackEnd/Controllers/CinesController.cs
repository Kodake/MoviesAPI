using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Enums;
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineDTO cineDTO)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var ubicacionCine = geometryFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));

            Cine cine = new Cine()
            {
                Nombre = "Coral Cine",
                Ubicacion = ubicacionCine,
                CineOferta = new CineOferta()
                {
                    PorcentajeDescuento = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(3)
                },
                SalasDeCine = new HashSet<SalaDeCine>()
                {
                    new SalaDeCine()
                    {
                        Precio = 200,
                        TipoSalaDeCine = TipoSalaDeCine.DosDimensiones
                    },
                    new SalaDeCine()
                    {
                        Precio = 300,
                        TipoSalaDeCine = TipoSalaDeCine.TresDimensiones
                    },
                }
            };

            _context.Add(cine);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("with-dto")]
        public async Task<ActionResult> PostWithDTO(CineCreacionDTO cineDTO)
        {
            var cine = _mapper.Map<Cine>(cineDTO);
            _context.Add(cine);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
