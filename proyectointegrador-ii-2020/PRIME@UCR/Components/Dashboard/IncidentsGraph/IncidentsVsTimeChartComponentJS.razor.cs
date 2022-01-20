using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsGraph
{
    public partial class IncidentsVsTimeChartComponentJS
    {
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }

        [Parameter] public bool ZoomActive { get; set; }

        private int eventQuantity { get; set; }

        [Inject]
        IModalService Modal { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GenerateColumnChart();
        }

        protected override async Task OnParametersSetAsync()
        {
            await GenerateColumnChart();
        }

        /*
         *  GenerateIncidentsVsTimeChartComponentJS()
         *  
         *  Method that generates the IncidentsVsTimeChartComponent  
         *  based on the incoming list from the dashboard service in which 
         *  data is already filtered 
         *  
         */
        private async Task GenerateColumnChart()
        {
            var incidentsData = Data.filteredIncidentsData;

            eventQuantity = incidentsData.Count();

            var incidentsPerDay = incidentsData.GroupBy(i => i.Cita.FechaHoraEstimada.DayOfYear).OrderBy((i) => i.Key);

            var results = new List<String>();

            foreach (var incidents in incidentsPerDay)
            {
                var date = incidents.First().Cita.FechaHoraEstimada.ToString().Substring(0, 10);
                results.Add(date);
                results.Add(incidents.ToList().Count().ToString());
            }


            await JS.InvokeVoidAsync("CreateIncidentsVsTimeChartComponent", (object)results);
        }

        void ShowModal()
        {
            var modalOptions = new ModalOptions()
            {
                Class = "graph-zoom-modal blazored-modal"
            };

            var parameters = new ModalParameters();
            parameters.Add(nameof(IncidentsVsTimeChartComponentJS.Data), Data);
            parameters.Add(nameof(IncidentsVsTimeChartComponentJS.ZoomActive), true);
            Modal.Show<IncidentsVsTimeChartComponentJS>("Incidentes en el tiempo", parameters, modalOptions);
        }
    }
}
