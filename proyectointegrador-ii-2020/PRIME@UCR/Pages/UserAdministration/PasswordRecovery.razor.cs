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
    public partial class PasswordRecovery
    {
        [Parameter]
        public string PasswordRecoveryToken1Encoded { get; set; }

        [Parameter]
        public string PasswordRecoveryToken2Encoded { get; set; }

        [Parameter]
        public string EmailEncoded { get; set; }

        [Inject]
        public UserManager<Usuario> UserManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public RecoveryPasswordInfoModel RecoveryPasswordInfo;

        public char ResultOfRecovery;

        public bool isBusy;

        protected override void OnInitialized()
        {
            RecoveryPasswordInfo = new RecoveryPasswordInfoModel();
        }

        protected override void OnParametersSet()
        {
            RecoveryPasswordInfo.EmailEncoded = EmailEncoded;
            RecoveryPasswordInfo.PasswordRecoveryToken1Encoded = PasswordRecoveryToken1Encoded;
            RecoveryPasswordInfo.PasswordRecoveryToken2Encoded = PasswordRecoveryToken2Encoded;
        }

        public async Task ChangePassword()
        {
            isBusy = true;
            var user = await UserManager.FindByEmailAsync(RecoveryPasswordInfo.Email);
            if (user != null)
            {
                var result = await UserManager.ResetPasswordAsync(user, RecoveryPasswordInfo.PasswordRecoveryToken, RecoveryPasswordInfo.PasswordModel.Password);
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
