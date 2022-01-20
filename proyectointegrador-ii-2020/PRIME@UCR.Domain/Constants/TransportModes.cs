using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Domain.Constants
{
    public static class TransportModes
    {
        public static readonly Modalidad GroundTransport = new Modalidad {Tipo = "Terrestre"};
        public static readonly Modalidad AirTransport = new Modalidad {Tipo = "Aéreo"};
        public static readonly Modalidad WaterTransport = new Modalidad {Tipo = "Marítimo"};
    }
}