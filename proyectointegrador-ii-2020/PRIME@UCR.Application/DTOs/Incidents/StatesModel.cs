using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Incidents
{
    public class StatesModel
    {
        public DateTime FechaHora { get; set; }
        public string AprobadoPor { get; set; }
        public string NombreEstado { get; set; }
        public Persona Aprobador { get; set; }
    }
}
