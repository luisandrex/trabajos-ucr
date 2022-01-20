using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class PermissionAuthorizeView
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public AuthorizationPermissions Permission { get; set; }
        [Parameter]
        public bool ShowDeniedPermissionMessage { get; set; } = true;
    }
}
