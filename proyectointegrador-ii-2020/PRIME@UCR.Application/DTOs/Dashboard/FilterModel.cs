using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.DTOs.Dashboard
{
    public class FilterModel
    {
        public FilterModel()
        {
            // final
            MedicalCenterDestination = new MedicalCenterLocationModel();
            HouseholdOriginFilter = new HouseholdModel();
            InternationalOriginFilter = new InternationalModel();
            MedicalCenterOriginFilter = new MedicalCenterLocationModel();

            // selected
            _selectedMedicalCenterDestination = new MedicalCenterLocationModel();
            _selectedHouseholdOrigin = new HouseholdModel();
            _selectedInternationalOrigin = new InternationalModel();
            _selectedMedicalCenterOrigin = new MedicalCenterLocationModel();
        }

        // final
        public Estado StateFilter { get; set; }
        public DateTime? InitialDateFilter { get; set; }
        public DateTime? FinalDateFilter { get; set; }
        public Modalidad ModalityFilter { get; set; }
        public MedicalCenterLocationModel MedicalCenterDestination { get; set; }
        public string OriginType { get; set; }
        public HouseholdModel HouseholdOriginFilter { get; set; }
        public InternationalModel InternationalOriginFilter { get; set; }
        public MedicalCenterLocationModel MedicalCenterOriginFilter { get; set; }

        // selected
        public Estado _selectedState { get; set; }
        public DateTime? _selectedInitialDate { get; set; }
        public DateTime? _selectedFinalDate { get; set; }
        public Modalidad _selectedModality { get; set; }
        public MedicalCenterLocationModel _selectedMedicalCenterDestination { get; set; }
        public string _selectedOriginType { get; set; }
        public HouseholdModel _selectedHouseholdOrigin { get; set; }
        public InternationalModel _selectedInternationalOrigin { get; set; }
        public MedicalCenterLocationModel _selectedMedicalCenterOrigin { get; set; }
        public bool ButtonEnabled { get; set; }
    }
}
