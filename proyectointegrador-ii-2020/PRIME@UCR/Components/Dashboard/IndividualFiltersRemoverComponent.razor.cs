using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard
{
    public partial class IndividualFiltersRemoverComponent
    {
        [Parameter]
        public FilterModel FilterInfo { get; set; }

        [Parameter]
        public EventCallback<FilterModel> FilterInfoChanged { get; set; }

        /*
         * Function: Method used to remove origin filter of type house holder.
         */
        private async Task RemoveHouseHoldOriginFilter()
        {
            FilterInfo.OriginType = null;
            FilterInfo._selectedOriginType = null;
            FilterInfo.HouseholdOriginFilter = new HouseholdModel();
            FilterInfo._selectedHouseholdOrigin = new HouseholdModel();
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of providence.
         */
        private async Task RemoveProvinceOriginFilter()
        {
            FilterInfo.HouseholdOriginFilter.District = null;
            FilterInfo._selectedHouseholdOrigin.District = null;
            FilterInfo.HouseholdOriginFilter.Canton = null;
            FilterInfo._selectedHouseholdOrigin.Canton = null;
            FilterInfo.HouseholdOriginFilter.Province = null;
            FilterInfo._selectedHouseholdOrigin.Province = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of canton.
         */
        private async Task RemoveCantonOriginFilter()
        {
            FilterInfo.HouseholdOriginFilter.District = null;
            FilterInfo._selectedHouseholdOrigin.District = null;
            FilterInfo.HouseholdOriginFilter.Canton = null;
            FilterInfo._selectedHouseholdOrigin.Canton = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of district.
         */
        private async Task RemoveDistrictOriginFilter()
        {
            FilterInfo.HouseholdOriginFilter.District = null;
            FilterInfo._selectedHouseholdOrigin.District = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of international type.
         */
        private async Task RemoveInternationalOriginFilter()
        {
            FilterInfo.OriginType = null;
            FilterInfo._selectedOriginType = null;
            FilterInfo._selectedInternationalOrigin = new InternationalModel();
            FilterInfo.InternationalOriginFilter = new InternationalModel();
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of country.
         */
        private async Task RemoveCountryOriginFilter()
        {
            FilterInfo.InternationalOriginFilter.Country = null;
            FilterInfo._selectedInternationalOrigin.Country = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of medical center type.
         */
        private async Task RemoveMedicalCenterOriginFilter()
        {
            FilterInfo.OriginType = null;
            FilterInfo._selectedOriginType = null;
            FilterInfo.MedicalCenterOriginFilter = new MedicalCenterLocationModel();
            FilterInfo._selectedMedicalCenterOrigin = new MedicalCenterLocationModel();
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove origin filter of specific medical center.
         */
        private async Task RemoveSpecificMedicalCenterOriginFilter()
        {
            FilterInfo.MedicalCenterOriginFilter = null;
            FilterInfo._selectedMedicalCenterOrigin = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove destination filter.
         */
        private async Task RemoveMedicalCenterDestinationFilter()
        {
            FilterInfo.MedicalCenterDestination.MedicalCenter = null;
            FilterInfo._selectedMedicalCenterDestination.MedicalCenter = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove modality filter.
         */
        private async Task RemoveModalityFilter()
        {
            FilterInfo.ModalityFilter = null;
            FilterInfo._selectedModality = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove state filter.
         */
        private async Task RemoveStateFilter()
        {
            FilterInfo.StateFilter = null;
            FilterInfo._selectedState = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove initial date filter.
         */
        private async Task RemoveInitialDateFilter()
        {
            FilterInfo.InitialDateFilter = null;
            FilterInfo._selectedInitialDate = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }

        /*
         * Function: Method used to remove initial date filter.
         */
        private async Task RemoveFinalDateFilter()
        {
            FilterInfo.FinalDateFilter = null;
            FilterInfo._selectedFinalDate = null;
            await FilterInfoChanged.InvokeAsync(FilterInfo);
        }
    }
}
