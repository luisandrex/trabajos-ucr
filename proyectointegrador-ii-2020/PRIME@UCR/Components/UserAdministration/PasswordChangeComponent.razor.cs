using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class PasswordChangeComponent
    {
        [Parameter]
        public ChangePasswordModel ChangePasswordModel { get; set; }

        [Parameter]
        public EventCallback<ChangePasswordModel> ChangePasswordModelChanged { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public EventCallback<bool> IsBusyChanged { get; set; }
    }
}
