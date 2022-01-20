using PRIME_UCR.Domain.Models.Incidents;
using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models
{
    public class Modalidad
    {
        public Modalidad() {
            Incidentes = new List<Incidente>();
            Unidades = new List<UnidadDeTransporte>();
        }
        public string Tipo { get; set; }
        public List<UnidadDeTransporte> Unidades { get; private set; }
        public List<Incidente> Incidentes { get; private set; }
    }

}