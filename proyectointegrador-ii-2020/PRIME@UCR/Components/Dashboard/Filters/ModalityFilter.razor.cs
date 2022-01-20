using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.Filters
{
    public partial class ModalityFilter
    {
        [Parameter] public FilterModel Value { get; set; }
        [Parameter] public EventCallback<FilterModel> ValueChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }

        private List<Modalidad> _modes;
        private bool _isLoading = true;
        private bool _changesMade = false;

        protected override void OnInitialized()
        {
            _modes = Data.modalities;
            _isLoading = false;
        }

        private void OnModalityChange(Modalidad modalidad) 
        {
            if (modalidad == Value.ModalityFilter)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            Value._selectedModality = modalidad;
        }

        private void Discard()
        {
            _changesMade = false;
            Value._selectedModality = Value.ModalityFilter;
        }

        private async Task Save()
        {
            StateHasChanged();
            Value.ModalityFilter = Value._selectedModality;
            if (Value.ModalityFilter != null)
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
