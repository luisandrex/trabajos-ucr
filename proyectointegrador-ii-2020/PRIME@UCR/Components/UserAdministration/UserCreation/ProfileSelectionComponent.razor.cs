using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration.UserCreation
{
    public partial class ProfileSelectionComponent
    {
        [Parameter]
        public RegisterUserFormModel Value { get; set; }

        public List<Perfil> ListProfiles { get; set; }

        [Inject]
        public IProfilesService profilesService { get; set; }

        protected override void OnInitialized()
        {
            ListProfiles = new List<Perfil>();
        }

        protected override async Task OnInitializedAsync()
        {
            ListProfiles = (await profilesService.GetPerfilesWithDetailsAsync()).ToList();
        }

        //Update the newUser model when different profiles are selected
        protected void update_profile(String profileName, ChangeEventArgs e)
        {
            if (Value.Profiles != null)
            {
                var Profile = (ListProfiles.Find(p => p.NombrePerfil == profileName));
                if ((bool)e.Value)
                {
                    Value.Profiles.Add(Profile.NombrePerfil);
                }
                else
                {
                    Value.Profiles.Remove(Profile.NombrePerfil);
                }

                foreach (String i in Value.Profiles)
                {
                    System.Diagnostics.Debug.WriteLine(i);
                }
            }
        }
    }
}
