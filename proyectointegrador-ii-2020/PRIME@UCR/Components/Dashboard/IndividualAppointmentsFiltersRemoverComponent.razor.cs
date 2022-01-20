using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard
{
    public partial class IndividualAppointmentsFiltersRemoverComponent
    {
        [Parameter]
        public AppointmentFilterModel AppointmentFilters { get; set; }

        [Parameter]
        public EventCallback<AppointmentFilterModel> AppointmentFiltersChanged { get; set; }

        /*
         * Method used to remove all hospital filters.
         */
        private async Task RemoveAllHospitalsFilter()
        {
            AppointmentFilters.Hospital.Clear();
            AppointmentFilters._selectedHospital = new MedicalCenterLocationModel();
            await AppointmentFiltersChanged.InvokeAsync(AppointmentFilters);
        }

        /*
         * Method used to remove a specific medical center from the filters.
         */
        private async Task RemoveSpecificHospital(MedicalCenterLocationModel hospital)
        {
            AppointmentFilters.Hospital.Remove(hospital);
            AppointmentFilters._selectedHospital = new MedicalCenterLocationModel();
            await AppointmentFiltersChanged.InvokeAsync(AppointmentFilters);
        }

        /*
         * Method used to remove all patients filters.
         */
        private async Task RemoveAllPatientsFilter()
        {
            AppointmentFilters.PatientModel.Clear();
            AppointmentFilters._selectedPatientModel = new PatientModel();
            await AppointmentFiltersChanged.InvokeAsync(AppointmentFilters);
        }

        /*
         * Method used to remove a specific patient from the filters.
         */
        private async Task RemoveSpecificPatient(PatientModel paciente)
        {
            AppointmentFilters.PatientModel.Remove(paciente);
            AppointmentFilters._selectedPatientModel = new PatientModel();
            await AppointmentFiltersChanged.InvokeAsync(AppointmentFilters);
        }
    }
}
