using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.StatePanel
{
    public partial class StateLog
    {
        [Parameter]
        public List<StatesModel> StatesLog { get; set; }
        private IncidentStatesList _states = new IncidentStatesList();
    }
}
