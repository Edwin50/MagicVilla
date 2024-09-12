using MagicVillaAPI.Models.DTO;

namespace MagicVillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto> {
            new VillaDto{Id=1, Nombre="Vista a Piscina" },
                    new VillaDto{Id=2, Nombre="Vista a la playa" },
                    new VillaDto{Id=3, Nombre="Vista a la montaña" }

        };
    }
}
