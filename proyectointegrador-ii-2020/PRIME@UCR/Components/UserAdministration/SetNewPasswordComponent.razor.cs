using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class SetNewPasswordComponent
    {
        [Parameter]
        public NewPasswordModel PasswordModel { get; set; }

        [Parameter]
        public EventCallback<NewPasswordModel> PasswordModelChanged { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public EventCallback<bool> IsBusyChanged { get; set; }


    }
}
