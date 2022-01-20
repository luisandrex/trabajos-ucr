using PRIME_UCR.Application.Dtos.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Constants
{
    public partial class IncidentSummary
    {
        public List<string> Values { get; set; }
        public List<string> Content { get; set; }
        public void LoadValues(IncidentDetailsModel Incident)
        {
            Values = new List<string> { Incident.Code, Incident.CurrentState + " ", Incident.EstimatedDateOfTransfer.ToString() };
            Content = new List<string> { "Incidente: ", "Estado: ", "Fecha estimada: " };
        }
    }
}
