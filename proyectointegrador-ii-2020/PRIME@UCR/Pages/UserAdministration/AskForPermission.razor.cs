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

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class AskForPermission
    {
        [Inject]
        public IPermissionsService permissionsService { get; set; }

        [Inject]
        public IProfilesService profilesService { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        [Inject]
        public UserManager<Usuario> UserManager { get; set; }


        [CascadingParameter]
        private Task<AuthenticationState> _authenticationState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IMailService mailService { get; set; }

        public AskForPermissionModel AskForPermissionMod;

        public bool isBusy;

        public char Inicialized = 'N';

        public char ResultOfRecovery = 'N';

        public List<Perfil> ListProfiles { get; set; }

        public List<Permiso> ListPermissions { get; set; }

        /*
         * Function: Initialices the AskForPermissionModel and the permissions and profiles lists
         */
        protected override void OnInitialized()
        {
            AskForPermissionMod = new AskForPermissionModel();
            isBusy = false;
            ListPermissions = new List<Permiso>();
            ListProfiles = new List<Perfil>();

        }

        /*
         * Function: Loads the user, and the permissions and profiles lists in order to fill the AskForPermissionModel
         *           with the corresponding assigned and unassigned permissions for the given user
         */
        protected override async Task OnInitializedAsync()
        {
            var userEmail = (await _authenticationState).User.Identity.Name;
            var user = await UserManager.FindByEmailAsync(userEmail);
            AskForPermissionMod.User = await userService.getUsuarioWithDetailsAsync(user.Id);
            ListPermissions = (await permissionsService.GetPermisos()).ToList();
            ListProfiles = (await profilesService.GetPerfilesWithDetailsAsync()).ToList();

            foreach (var profile in AskForPermissionMod.User.UsuariosYPerfiles)
            {
                var currProfile = ListProfiles.Find(p => profile.IDPerfil == p.NombrePerfil);
                foreach (var permission in currProfile.PerfilesYPermisos)
                {
                    if (AskForPermissionMod.AssignedPermissions.Find(p => permission.IDPermiso == p.IDPermiso) != null ? false : true)
                    {
                        AskForPermissionMod.AssignedPermissions.Add(permission.Permiso);
                    }
                }
            }

            foreach (var permission in ListPermissions)
            {
                var permissionAssigned = AskForPermissionMod.AssignedPermissions.Find(p => permission.IDPermiso == p.IDPermiso) == null ? false : true;
                if (permissionAssigned == false)
                {
                    AskForPermissionMod.NotAssignedPermissions.Add(permission);
                }
            }

            Inicialized = 'T';
        }

        /*
         * Function:        Method used to send the message provided by the user (asking for a permission) to the administrators
         */
        public async Task AskPermission()
        {
            isBusy = true;
            StateHasChanged();
            var listUsers = (await userService.GetAllUsersWithDetailsAsync()).ToList();
            if (AskForPermissionMod.User != null && listUsers != null)
            {
                var userPerson = (await userService.GetAllUsersWithDetailsAsync()).ToList().Find(u => u.Email == AskForPermissionMod.User.Email);
                foreach (var actUser in listUsers)
                {
                    if (actUser.UsuariosYPerfiles.Find(p => p.Perfil.NombrePerfil == "Administrador") == null ? false : true)
                    {
                        var url = $"{NavigationManager.BaseUri}/user_administration/profiles";
                        var message = new EmailContentModel()
                        {
                            Destination = actUser.Email,
                            Subject = "PRIME@UCR: Ha recibido una solicitud de permiso",
                            Body = $"<p>Estimado(a) {actUser.Persona.Nombre}, el usuario {userPerson.Persona.Nombre} ha solicitado el(los) permiso(s) \"{string.Join(",", AskForPermissionMod.PermissionsList)}\". Si desea otogarle el(los) permiso(s), favor presione <a href=\"{url}\">aquí</a>.</p>"
                        };

                        await mailService.SendEmailAsync(message);
                    }

                }
                ResultOfRecovery = 'T';
                AskForPermissionMod.UserMessage = String.Empty;
                AskForPermissionMod.StatusMessage = "Su solicitud ha sido enviada.";
                AskForPermissionMod.StatusMessageType = "success";

            }
            else
            {
                ResultOfRecovery = 'F';
            }
            isBusy = false;
            StateHasChanged();

        }
    }
}
