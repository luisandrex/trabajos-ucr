using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsCounter
{

    public partial class TotalIncidentsCounterComponent
    {
        [Parameter]
        public IncidentsCounterModel IncidentsCounter { get; set; }
    }
}

