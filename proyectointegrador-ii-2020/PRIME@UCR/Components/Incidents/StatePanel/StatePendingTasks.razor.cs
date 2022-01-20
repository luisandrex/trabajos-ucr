using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.StatePanel
{
    public partial class StatePendingTasks
    {
        [Parameter]
        public List<Tuple<string, string>> PendingTasks { get; set; }
        [Parameter]
        public string NextState { get; set; }
        [Parameter]
        public IncidentDetailsModel Incident { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }

        public void RedirectToTab(string url)
        {
            NavManager.NavigateTo($"{"/incidents/" + Incident.Code + "/" + url}", forceLoad: true);
        }

        private string toRedirectTab(string tabName)
        {
            switch (tabName)
            {
                case "Info":
                    return "Resumen";
                case "Origin":
                    return "Origen";
                case "Destination":
                    return "Destino";
                case "Patient":
                    return "Paciente";
                case "Assignment":
                    return "Asignación";
                case "Multimedia":
                    return "Multimedia";
                case "Checklist":
                    return "Listas de chequeo";
                default:
                    return "Resumen";       //common case
            }
        }
    }
}
