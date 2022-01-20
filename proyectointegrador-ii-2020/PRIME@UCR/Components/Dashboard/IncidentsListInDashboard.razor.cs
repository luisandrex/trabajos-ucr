using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Implementations.Dashboard;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Components.Incidents.IncidentDetails.Tabs;
using PRIME_UCR.Domain.Models;


namespace PRIME_UCR.Components.Dashboard
{
    public partial class IncidentsListInDashboard
    {
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        private ITable<Incidente> Table;
        private bool _isLoading = true;

        protected override void OnInitialized()
        {
            _isLoading = false;            
        }

        protected override void OnParametersSet()
        {
            _isLoading = true;
            StateHasChanged();
            _isLoading = false;
        }
    }

}

