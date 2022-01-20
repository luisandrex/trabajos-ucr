using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.Filters
{
    public partial class StateFilter
    {
        [Parameter] public FilterModel Value { get; set; }
        [Parameter] public EventCallback<FilterModel> ValueChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }

        private List<Estado> _stateTypes;
        private bool _isLoading = true;
        private bool _changesMade = false;

        protected override void OnInitialized()
        {
            _stateTypes = Data.states;
            _isLoading = false;
        }

        private void OnStateChange(Estado state) 
        {
            if (state == Value.StateFilter)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            Value._selectedState = state;
        }

        private void Discard()
        {
            _changesMade = false;
            Value._selectedState = Value.StateFilter;
        }

        private async Task Save()
        {
            StateHasChanged();
            Value.StateFilter = Value._selectedState;
            if (Value.StateFilter != null)
            {
                Value.ButtonEnabled = true;
            }
            else
            {
                Value.ButtonEnabled = false;
            }
            _changesMade = false;
            await ValueChanged.InvokeAsync(Value);
        }

    }

}