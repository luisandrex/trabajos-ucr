using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Incidents
{
    public class LastChangeModel
    {
        public Persona Responsable { get; set; }
        public string CodigoIncidente { get; set; }
        public DateTime FechaHora { get; set; }
        public string UltimoCambio { get; set;}
    }
}
