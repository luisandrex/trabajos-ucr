using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class UserAccountValidation
    {
        [Parameter]
        public string EmailEncoded { get; set; }

        [Parameter]
        public string Code1Encoded { get; set; }

        [Parameter]
        public string Code2Encoded { get; set; }

        [Inject]
        public UserManager<Usuario> UserManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        
        public UserValidationInfoModel UserValidationInfo;

        public char ValidationState = 'N';

        public char ResultOfRecovery;

        public bool isBusy;


        protected override async Task OnParametersSetAsync()
        {
            UserValidationInfo = new UserValidationInfoModel();
            UserValidationInfo.EmailEncoded = EmailEncoded;
            UserValidationInfo.Code1Encoded = Code1Encoded;
            UserValidationInfo.Code2Encoded = Code2Encoded;
            await ValidateUserAsync();
        }

        public async Task ValidateUserAsync()
        {
            var user = await UserManager.FindByEmailAsync(UserValidationInfo.Email);
            var result = await UserManager.ConfirmEmailAsync(user, UserValidationInfo.EmailValidationToken);
            if(result.Succeeded)
            {
                ValidationState = 'T';
            }
            else 
            {
                ValidationState = 'F';
            }
        }

        public async Task SetPassword()
        {
            isBusy = true;
            var user = await UserManager.FindByEmailAsync(UserValidationInfo.Email);
            if (user != null)
            {
                var result = await UserManager.AddPasswordAsync(user, UserValidationInfo.PasswordModel.Password);
                ResultOfRecovery = result.Succeeded ? 'T' : 'F';
            }
            else
            {
                ResultOfRecovery = 'F';
            }
            isBusy = false;
            StateHasChanged();
            if (ResultOfRecovery == 'F')
            {
                return;
            }
            await Task.Delay(2000);
            NavigationManager.NavigateTo("/");
        }
    }
}
