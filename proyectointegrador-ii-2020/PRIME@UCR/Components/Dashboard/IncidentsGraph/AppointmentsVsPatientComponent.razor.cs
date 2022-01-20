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
    public partial class AppointmentsVsPatientComponent
    {
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }

        [Parameter] public bool ZoomActive { get; set; }


        private int patientQuantity { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GenerateCreateAppointmentVsPatientComponent();
        }


        protected override async Task OnParametersSetAsync()
        {
            await GenerateCreateAppointmentVsPatientComponent();
        }

        /*
         *  GenerateCreateAppointmentVsPatientComponent()
         *  
         *  Method that generates the AppointmentVsPatient Graph 
         *  based on the incoming list from the dashboard service in which 
         *  data is already filtered 
         *  
         */
        private async Task GenerateCreateAppointmentVsPatientComponent()
        {

            var appointmentList = Data.filteredAppointmentsData;

            var appointmentsPerPatient = appointmentList.GroupBy(a => a.Cita.Expediente.CedulaPaciente);

            patientQuantity = appointmentsPerPatient.Count();
            var results = new List<String>();
            foreach (var patientA in appointmentsPerPatient)
            {
                
                var labelName = "No Asignado";
                if (patientA != null)
                {
                    if (patientA.Any())
                    {
                        labelName = patientA.First().Cita.Expediente.CedulaPaciente;
                    }
                }
                results.Add(labelName);
                results.Add(patientA.Count().ToString());
            }

            await JS.InvokeVoidAsync("CreateAppointmentVsPatientComponentJS", (object)results);
        }

        void ShowModal()
        {
            var modalOptions = new ModalOptions()
            {
                Class = "graph-zoom-modal blazored-modal"
            };

            var parameters = new ModalParameters();
            parameters.Add(nameof(AppointmentsVsPatientComponent.Data), Data);
            parameters.Add(nameof(AppointmentsVsPatientComponent.ZoomActive), true);
            Modal.Show<AppointmentsVsPatientComponent>("Citas por Paciente", parameters, modalOptions);
        }
    }
}
