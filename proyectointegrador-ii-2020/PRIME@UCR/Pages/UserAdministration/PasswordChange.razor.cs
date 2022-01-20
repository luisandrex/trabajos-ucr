using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class PasswordChange
    {
        [Parameter]
        public string OldPassword { get; set; }

        [Parameter]
        public string NewPassword { get; set; }

        [Parameter]
        public string NewPasswordConfirm { get; set; }

        [Inject]
        public UserManager<Usuario> UserManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ChangePasswordModel ChangePasswordModel;

        public bool isBusy;
        
        public char ValidationState = 'N';

        public char ResultOfRecovery = 'N';

        public char NewPasswordConfirmed = 'N';

        public char OldPasswordConfirmed = 'N';


        /*
         * Function: Initialices the ChangePasswordModel with the given parameters and calls the function to validate user
         */
        protected override async Task OnParametersSetAsync()
        {
            ChangePasswordModel = new ChangePasswordModel();
            ChangePasswordModel.OldPassword = OldPassword;
            ChangePasswordModel.NewPassword = NewPassword;
            ChangePasswordModel.NewPasswordConfirm = NewPasswordConfirm;
            await ValidateUser();
        }

        /*
         * Function: Validates the user (checks that the given user is authorized)
         */
        public async Task ValidateUser()
        {
            var emailUser = (await authenticationState).User.Identity.Name;
            var user = await UserManager.FindByEmailAsync(emailUser);

            if (user != null )
            {
                ValidationState = 'T';
            }
            else
            {
                ValidationState = 'F';
            }
        }

        /*
         * Function: Validates the current password, new password and its confirmation. 
         *           Given that they are correct, the users password is changed. Otherwise, a message with feedback is displayed
         */
        public async Task ChangePassword()
        {
            isBusy = true;

            var emailUser = (await authenticationState).User.Identity.Name;

            var user = await UserManager.FindByEmailAsync(emailUser);

            if (user != null)
            {
                var correctPass = await UserManager.CheckPasswordAsync(user, ChangePasswordModel.OldPassword);
                if (correctPass == false)
                {
                    OldPasswordConfirmed = 'F';
                }
                else
                {
                    OldPasswordConfirmed = 'T';
                    if (ChangePasswordModel.NewPassword != ChangePasswordModel.NewPasswordConfirm)
                    {
                        NewPasswordConfirmed = 'F';
                    }
                    else
                    {
                        NewPasswordConfirmed = 'V';
                        var result = await UserManager.ChangePasswordAsync(user, ChangePasswordModel.OldPassword, ChangePasswordModel.NewPassword);
                        ResultOfRecovery = result.Succeeded ? 'T' : 'F';
                    }
                }
                    
            }
            else
            {
                ResultOfRecovery = 'F';
            }
            isBusy = false;
            StateHasChanged();
            if (ResultOfRecovery != 'T')
            {
                return;
            }

        }
    }
}
