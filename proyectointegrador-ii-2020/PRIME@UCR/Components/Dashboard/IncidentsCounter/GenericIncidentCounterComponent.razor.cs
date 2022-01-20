using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsCounter
{
    public partial class GenericIncidentCounterComponent
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public int Counter { get; set; }
    }
}
