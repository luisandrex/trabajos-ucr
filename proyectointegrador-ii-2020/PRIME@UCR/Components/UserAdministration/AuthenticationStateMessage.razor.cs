using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Components.Controls;

namespace PRIME_UCR.Components.UserAdministration
{
    /*
     * Class used to select the message to show in authorization view based on the authentication state.
     */
    public partial class AuthenticationStateMessage
    {
        [CascadingParameter]
        private Task<AuthenticationState> _authenticationState { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Parameter]
        public AuthorizationPermissions? Permission { get; set; } = null; 

        bool isAuthenticated = false;

        /*
         * Function:        Method used to get the message that is going to be displayed to the user based on its authentication state.
         * 
         * Returns:         A string indicating if the user could access the page because of its user or in its permissions.
         */
        private async Task<string> GetMessageAsync()
        {
            var result = (await _authenticationState).User;
            if (result.Identity.IsAuthenticated)
            {
                isAuthenticated = true;
                string deniedMessage;
                if (Permission != null)
                {
                    deniedMessage = $"Para acceder a esta página debe contar con el permiso de {EnumUtils.GetDescription(Permission)}.";
                }
                else 
                {
                    deniedMessage = "Para acceder a esta página debe contar con los permisoss necesarios";
                }
                return deniedMessage;
            }
            else
            {
                return "Para acceder a esta página debe registarse.";
            }
        }

        private void goToPageAskPermission()
        {
            navigationManager.NavigateTo("/AskPermission");
        }
    }
}
