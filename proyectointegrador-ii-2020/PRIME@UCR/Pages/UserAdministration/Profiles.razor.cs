using System;
using System.Collections.Generic;
using System.Linq;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.UserAdministration;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class Profiles
    {
        [Inject]
        public IPermissionsService permissionsService { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        public ProfileModel profile;

        public bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            profile = new ProfileModel();

            profile.ProfileName = ""; /*Cambiar una vez que se tenga el drop down de Diosvier*/

            var permissionsList = await permissionsService.GetPermisos();
            profile.CheckedPermissions = new List<bool>();
            foreach (var permission in permissionsList) 
            {
                profile.CheckedPermissions.Add(false);
            }
            profile.CheckedUsers = new List<Tuple<string,bool>>();
            var usersList = await userService.GetAllUsersWithDetailsAsync();
            foreach(var user in usersList)
            {
                profile.CheckedUsers.Add(new Tuple<string, bool>(user.Id, false));
            }
            profile.PermissionsList = new List<Permiso>();
            profile.UserLists = new List<Usuario>();

        }

    }
}
