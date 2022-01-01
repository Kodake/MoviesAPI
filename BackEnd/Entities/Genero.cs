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
        public HashSet<Pelicula> Peliculas { get; set; }

        public static implicit operator Genero(List<Genero> v)
        {
            throw new NotImplementedException();
        }
    }
}
