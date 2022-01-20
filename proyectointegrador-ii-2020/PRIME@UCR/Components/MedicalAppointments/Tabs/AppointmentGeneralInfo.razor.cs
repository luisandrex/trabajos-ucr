using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Components.MedicalRecords.Constants;

namespace PRIME_UCR.Components.MedicalAppointments.Tabs
{
    public partial class AppointmentGeneralInfo
    {
        [Parameter] public CitaMedica Appointment { get; set; }

        [Parameter] public Paciente Pacient { get; set; }

        public RecordSummary Summary;
        public Médico doctor { get; set; }

        public EstadoCitaMedica current_state { get; set; }


        public CentroMedico medical_center { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //current_state = await appointment_service.GetMedAppointmentStatusAsync(Appointment.EstadoId);

            doctor = await doctor_service.GetDoctorByIdAsync(Appointment.CedMedicoAsignado);
            current_state = (await appointment_service.GetStatusById(Appointment.EstadoId));
            Summary = new RecordSummary();
            Summary.LoadPatientValues(Pacient);
        }

        public async void NextStatus()
        {
            if (current_state.NombreEstado != "Finalizada")
            { 
                await appointment_service.UpdateAppointmentStatus(Appointment.Id);
                Appointment = await appointment_service.GetMedicalAppointmentWithAppointmentByIdAsync(Appointment.Id);
                current_state = (await appointment_service.GetStatusById(Appointment.EstadoId));
                StateHasChanged();
            }
        }

    }
}
