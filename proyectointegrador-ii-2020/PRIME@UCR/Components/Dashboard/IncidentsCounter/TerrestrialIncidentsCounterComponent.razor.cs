using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsCounter
{
    public partial class TerrestrialIncidentsCounterComponent
    {
        [Parameter]
        public IncidentsCounterModel IncidentsCounter { get; set; }
    }
}
