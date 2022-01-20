using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.DTOs.Dashboard
{
    public class AppointmentFilterModel
    {
        public AppointmentFilterModel()
        {
            // final
            Hospital = new List<MedicalCenterLocationModel>();
            PatientModel = new List<PatientModel>();
            // selected
            _selectedHospital = new MedicalCenterLocationModel();
            _selectedPatientModel = new PatientModel();
        }

        // final
        public List<MedicalCenterLocationModel> Hospital { get; set; }
        public List<PatientModel> PatientModel { get; set; }

        // selected
        public MedicalCenterLocationModel _selectedHospital { get; set; }
        public PatientModel _selectedPatientModel { get; set; }

        public bool ButtonEnabled { get; set; }
    }
}
