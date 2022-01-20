using Microsoft.AspNetCore.Components;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.StatePanel
{
    public partial class StateProgressBar
    {
        [Parameter]
        public int CurrentStateIndex { get; set; }

        private readonly IncidentStatesList _states = new IncidentStatesList();

        public string setStateColor(int index)
        {
            string className = "";
            if (index < CurrentStateIndex)
            {
                className = "bg-primary border  border-light";
            }
            else if (index > CurrentStateIndex)
            {
                className = "bg-secondary border border-light";
            }
            else
            {
                className = "bg-current-state border border-light";
            }
            return className;
        }

        public string setProgressIndicator(int index)
        {
            return index == CurrentStateIndex ? "bi bi-caret-down-fill indicator-color" : "bi bi-caret-down-fill invisible";
        }

    }
}
