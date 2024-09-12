using System.ComponentModel.DataAnnotations;

namespace MagicVillaAPI.Models.DTO
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required,MaxLength(150)]
        public string? Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public string ImagenURL { get; set; }
     


    }
}
