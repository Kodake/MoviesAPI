using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs
{
    public class CineOfertaCreacionDTO
    {
        [Range(1,100)]
        public double PorcentajeDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
