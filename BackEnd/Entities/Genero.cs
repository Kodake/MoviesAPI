using System.ComponentModel.DataAnnotations;

namespace BackEnd.Entities
{
    public class Genero
    {
        //[Key]
        public int Identificador { get; set; }
        //[StringLength(100)] o //[MaxLength(100)]
        //[Required]
        public string Nombre { get; set; }
        public bool EstaBorrado { get; set; }
        public HashSet<Pelicula> Peliculas { get; set; }
    }
}
