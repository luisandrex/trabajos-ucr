using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Incidents
{
    public class CambioIncidente
    {
        public string CedFuncionario { get; set; }
        public string CodigoIncidente { get; set; }
        public Incidente Incidente { get; set; }
        public DateTime FechaHora { get; set; }
        public string UltimoCambio { get; set; }

    }
}