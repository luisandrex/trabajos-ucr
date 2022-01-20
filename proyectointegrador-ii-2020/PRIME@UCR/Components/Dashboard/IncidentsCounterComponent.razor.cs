using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard
{
    public partial class IncidentsCounterComponent
    {
        [Parameter]
        public IncidentsCounterModel Value { get; set; }
        [Parameter]
        public EventCallback<IncidentsCounterModel> ValueChanged { get; set; }

        [Inject]
        public IDashboardService DashboardService { get; set; }

        private string _selectedFilter = "Día";

        private readonly List<string> _filters = new List<string> { "Día", "Semana", "Mes", "Año" };        

        private async Task OnFilterChange(string filter)
        {
            Value.isReadyToShowCounters = false;
            _selectedFilter = filter;
            Value.totalIncidentsCounter = await DashboardService.GetIncidentCounterAsync(String.Empty, filter);
            Value.maritimeIncidents = await DashboardService.GetIncidentCounterAsync("Marítimo", filter);
            Value.airIncidentsCounter = await DashboardService.GetIncidentCounterAsync("Aéreo", filter);
            Value.groundIncidentsCounter = await DashboardService.GetIncidentCounterAsync("Terrestre", filter);
            Value.isReadyToShowCounters = true;
        }
    }
}
