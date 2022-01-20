using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsGraph
{
    public partial class GraphContainer
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string QuantityTitle { get; set; }

        [Parameter]
        public int EventQuantity { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback ShowModal { get; set; }

        [Parameter]
        public bool ZoomActive { get; set; }

        [Parameter]
        public bool IsAppointment { get; set; }
    }
}
