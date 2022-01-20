using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class UserAdministrationMenuComponent
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string ProfileNavStatus = "";
        public string CreateUserStatus = "";
        public string ResendInvitationStatus = "";

        protected override void OnInitialized()
        {
            string url = NavigationManager.Uri;
            var actualTab = url.Split('/')[4];
            if(actualTab == "profiles")
            {
                ProfileNavStatus = "active";
            }
            else if (actualTab == "registerUser")
            {
                CreateUserStatus = "active";
            } else
            {
                ResendInvitationStatus = "active";
            }
        }
    }
}
