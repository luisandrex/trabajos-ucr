using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class AskForPermissionComponent
    {

        [Parameter]
        public AskForPermissionModel AskForPermissionModel { get; set; }

        [Parameter]
        public EventCallback<AskForPermissionModel> AskForPermissionModelChanged { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public EventCallback<bool> IsBusyChanged { get; set; }

        public List<Permiso> ListPermissions { get; set; }

        public List<Permiso> NotAssignedPermissions { get; set; }

        public List<Permiso> AssignedPermissions { get; set; }

        public List<Perfil> ListProfiles { get; set; }

        /*
         * Function: Update the list of permissions that are requested
         */
        protected void update_list(int idPermission, ChangeEventArgs e)
        {
            var permission = AskForPermissionModel.NotAssignedPermissions.Find(p => idPermission == p.IDPermiso);
            if(permission != null)
            {
                if ((bool)e.Value)
                {
                    AskForPermissionModel.PermissionsList.Add($"{permission.IDPermiso}: {permission.DescripciónPermiso}");
                }
                else
                {
                    AskForPermissionModel.PermissionsList.Remove($"{permission.IDPermiso}: {permission.DescripciónPermiso}");
                }
            }
        }


    }
}
