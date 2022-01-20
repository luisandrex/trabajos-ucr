using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;

namespace PRIME_UCR.Components.Incidents
{
    public partial class AuthorizeIfAssigned
    {

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        private IAssignmentService AssignmentService { get; set; }

        [Parameter]
        public string IncidentCode { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool? _isAuthorized;

        protected override async Task OnInitializedAsync()
        {
            // check if the logged in user is authorized to view this patient's details
            var emailUser = (await AuthenticationState).User.Identity.Name;
            var user = await UserService.getPersonWithDetailstAsync(emailUser);
            _isAuthorized = user != null &&
                            await AssignmentService.IsAuthorizedToViewPatient(IncidentCode, user.Cédula);

        }
    }
}
