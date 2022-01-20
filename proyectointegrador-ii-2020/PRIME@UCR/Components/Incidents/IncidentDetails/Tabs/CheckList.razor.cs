using MatBlazor;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public partial class CheckList
    {
        [Parameter] public IncidentDetailsModel Incident { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        // Info for Incident summary that is shown at top of the page
        public IncidentSummary Summary = new IncidentSummary();

        MatTheme AddButtonTheme = new MatTheme()
        {
            Primary = "white",
            Secondary = "#095290"
        };

        protected override async Task OnInitializedAsync()
        {
            Summary.LoadValues(Incident);
        }
        void Redirect()
        {
            NavManager.NavigateTo($"/checklist/{Incident.Code}");
        }
    }
}
