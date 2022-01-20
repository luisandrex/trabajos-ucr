using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Incidents.IncidentDetails.Tabs;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Pages.Incidents
{
    public partial class IncidentList
    {
        private const string CreateIncidentUrl = "/incidents/create";

        [Inject]
        private NavigationManager NavManager { get; set; }

        private ITable<IncidentListModel> Table;

        private List<IncidentListModel> incidentsList;

        [Inject]
        private IIncidentService IncidentService { get; set; }

        [Inject]
        private IPersonService PersonService { get; set; }

        [Inject] public IUserService UserService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        private bool _isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            var user = await AuthenticationState;
            var email = user.User.Identity.Name;


            var _currentUser = await UserService.getPersonWithDetailstAsync(email);
            var person = await PersonService.GetPersonByIdAsync(_currentUser.Cédula);

            incidentsList = (await IncidentService.GetIncidentListModelsAsync(person.Cédula)).ToList();

            if(incidentsList != null)
            {
                _isLoading = false;
            }

        }


        MatTheme AddButtonTheme = new MatTheme()
        {
            Primary = "white",
            Secondary = "#095290"
        };


        void Redirect()
        {
            NavManager.NavigateTo($"{CreateIncidentUrl}");
        }

        public void IsLoading(bool loadingValue)
        {
            _isLoading = loadingValue;
        }

    }

}




