using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models
{
    public class Domicilio : Ubicacion
    {
        public string Direccion { get; set; }
        public int DistritoId { get; set; }
        public Distrito Distrito { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public override string DisplayName => "Domicilio";
    }
}