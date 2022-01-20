using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.UserAdministration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace PRIME_UCR.Components.UserAdministration.ProfilesModifications
{
    /**
     * Partial class used to manage the logic part of the PermissionsComponent.
     */
    public partial class PermissionsComponent
    {

        [Inject]
        public IPermissionsService permissionsService { get; set; }

        [Inject]
        public IPermiteService permiteService { get; set; }

        [Inject]
        public IMailService mailService { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }

        public List<Permiso> ListPermissions { get; set; }

        [Parameter]
        public ProfileModel Value { get; set; }

        [Parameter]
        public EventCallback<ProfileModel> ValueChanged { get; set; }
        
        [Parameter]
        public bool isLoading { get; set; }

        [Parameter]
        public EventCallback<bool> isLoadingChanged { get; set; }
        /**
         * Function: Assigns, a new list of permissions to the attribute ListPermissions once IsInitialized  is set to true.
         */
        protected override void OnInitialized()
        {
            ListPermissions = new List<Permiso>();
        }

        /**
         * Function: Assigns, a new list of permissions to the attribute ListPermissions based on the permissions that are
         * placed on the database.
         */
        protected override async Task OnInitializedAsync()
        {
            ListPermissions = (await permissionsService.GetPermisos()).ToList();
        }

        /**
         * Function: Used to update the permissions asigned to each profile so, the permissions chosen by the user on the
         * front end, are changed at the database level. 
         * 
         * Requires: The permission id, which corresponds to a attribute that uniquely identifies a permission, and an argument
         * that indicates that there's an event happening.
         */
        protected async Task update_profile(int idPermission, ChangeEventArgs e)
        {
            isLoading = true;
            await isLoadingChanged.InvokeAsync(isLoading);
            if (Value.PermissionsList != null)
            {
                var Permission = (ListPermissions.Find(p => p.IDPermiso == idPermission));

                if ((bool)e.Value)
                {
                    Value.PermissionsList.Add(Permission);
                    await permiteService.InsertPermissionAsync(Value.ProfileName, idPermission);
                    foreach(var user in Value.UserLists)
                    {
                        var userPerson = (await userService.GetAllUsersWithDetailsAsync()).ToList().Find(u => u.Email == user.Email);
                        var message = new EmailContentModel()
                        {
                            Destination = user.Email,
                            Subject = "PRIME@UCR: Se le ha otorgado un nuevo permiso",
                            Body = $"<p>Estimado(a) {userPerson.Persona.Nombre}, el permiso \"{Permission.DescripciónPermiso}\" ha sido agregado al perfil \"{Value.ProfileName}\". Por lo tanto, a partir de este momento, usted posee dicho permiso en su cuenta de la aplicación PRIME@UCR. </p>"
                        };

                        await mailService.SendEmailAsync(message);

                    }
                    Value.StatusMessage = "El permiso \"" + Permission.DescripciónPermiso + "\" fue agregado al perfil " + Value.ProfileName + " y se ha notificado a los usuarios." ;
                    Value.StatusMessageType = "success";
                }
                else 
                {
                    Value.PermissionsList.Remove(Permission);
                    await permiteService.DeletePermissionAsync(Value.ProfileName,idPermission);
                    foreach (var user in Value.UserLists)
                    {
                        var userPerson = (await userService.GetAllUsersWithDetailsAsync()).ToList().Find(u => u.Email == user.Email);
                        var message = new EmailContentModel()
                        {
                            Destination = user.Email,
                            Subject = "PRIME@UCR: Se le ha removido un permiso",
                            Body = $"<p>Estimado(a) {userPerson.Persona.Nombre}, el permiso \"{Permission.DescripciónPermiso}\" ha sido removido del perfil \"{Value.ProfileName}\". Por lo tanto, a partir de este momento, usted no posee dicho permiso en su cuenta de la aplicación PRIME@UCR. </p>"
                        };

                        await mailService.SendEmailAsync(message);

                    }
                    Value.StatusMessage = "El permiso \"" + Permission.DescripciónPermiso + "\" fue removido del perfil " + Value.ProfileName + " y se ha notificado a los usuarios.";
                    Value.StatusMessageType = "warning";
                }
                await authenticationStateProvider.GetAuthenticationStateAsync();
                Value.CheckedPermissions[idPermission-1] = (bool)e.Value;
                await ValueChanged.InvokeAsync(Value);
            }
            isLoading = false ;
            await isLoadingChanged.InvokeAsync(isLoading);
        }
    }
}