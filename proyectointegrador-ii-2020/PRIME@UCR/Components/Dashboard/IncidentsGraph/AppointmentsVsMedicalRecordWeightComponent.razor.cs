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
    public partial class AppointmentsVsMedicalRecordWeightComponent
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
            var appointmentsList = Data.filteredAppointmentsData;

            var appointmentsPerPatient = appointmentsList.GroupBy(a => a.Cita.Expediente.CedulaPaciente);

            appointmentsQuantity = appointmentsList.Count();
            var results = new List<List<string>>();

            foreach (var patientA in appointmentsPerPatient)
            {
                var tempResult = new List<string>();
                foreach(var appointment in patientA)
                {
                    tempResult.Add(appointment.Cita.FechaHoraEstimada.ToString().Substring(0, 10));
                    tempResult.Add(appointment.Cita.Metricas.First().Peso);
                }
                results.Add(new List<String>() { patientA.First().Cita.Expediente.CedulaPaciente });
                results.Add(tempResult);
            }


            await JS.InvokeVoidAsync("CreateAppointmentsVsMedicalRecordsWeightComponentJS", (object)results);
        }

        void ShowModal()
        {
            var modalOptions = new ModalOptions()
            {
                Class = "graph-zoom-modal blazored-modal"
            };

            var parameters = new ModalParameters();
            parameters.Add(nameof(AppointmentsVsMedicalRecordWeightComponent.Data), Data);
            parameters.Add(nameof(AppointmentsVsMedicalRecordWeightComponent.ZoomActive), true);
            Modal.Show<AppointmentsVsMedicalRecordWeightComponent>("Evolucion Metricas de Paciente", parameters, modalOptions);
        }
    }
}
