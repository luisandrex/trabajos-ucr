using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsGraph
{
    public partial class AppointmentsVsMedicalCenterComponent
    {
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }

        [Parameter] public bool ZoomActive { get; set; }


        private int appointmentsQuantity { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GenerateAppointmentsVsMedicalCenterComponent();
        }


        protected override async Task OnParametersSetAsync()
        {
            await GenerateAppointmentsVsMedicalCenterComponent();
        }

        /*
         *  GenerateAppointmentsVsMedicalCenterComponent()
         *  
         *  Method that generates the IncidentsVsDestination Graph 
         *  based on the incoming list from the dashboard service in which 
         *  data is already filtered 
         *  
         */
        private async Task GenerateAppointmentsVsMedicalCenterComponent()
        {
            //TODO: Use Cita Data to create this
            var appointments = Data.filteredAppointmentsData;

            appointmentsQuantity = appointments.Count();
            var medicalCenters = Data.medicalCenters;

            var appointmentDestination = appointments.GroupBy(a => {
                if (a.CedMedicoAsignado != null)
                {
                    return a.CentroMedicoId;
                }
                else
                {
                    return 0;
                }
            });

            var results = new List<String>();
            foreach (var appointmentsItem in appointmentDestination)
            {
                if (appointmentsQuantity > 0)
                {
                    var labelName = "No Asignado";


                    var selectedMedicalCenter = medicalCenters.Where((medCenter) => appointmentsItem.ToList().First().CentroMedicoId == medCenter.Id);
                    if (selectedMedicalCenter.Any())
                    {
                        labelName = selectedMedicalCenter.First().Nombre;
                    }

                    results.Add(labelName);
                    results.Add(appointmentsItem.ToList().Count().ToString());
                }
                //results.Add(incidents.ToList().Count().ToString());
            }

            await JS.InvokeVoidAsync("CreateAppointmentsVsMedicalCenterComponentJS", (object)results);
        }

        void ShowModal()
        {
            var modalOptions = new ModalOptions()
            {
                Class = "graph-zoom-modal blazored-modal"
            };

            var parameters = new ModalParameters();
            parameters.Add(nameof(AppointmentsVsMedicalCenterComponent.Data), Data);
            parameters.Add(nameof(AppointmentsVsMedicalCenterComponent.ZoomActive), true);
            Modal.Show<AppointmentsVsMedicalCenterComponent>("Citas por Centro Medico", parameters, modalOptions);
        }
    }
}
