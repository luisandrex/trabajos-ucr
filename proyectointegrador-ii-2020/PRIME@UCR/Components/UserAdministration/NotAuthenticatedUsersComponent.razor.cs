using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration
{
    public partial class NotAuthenticatedUsersComponent
    {
        [Inject]
        public IUserService userService { get; set; }

        [Inject]
        public IMailService mailService { get; set; }

        [Inject]
        public UserManager<Usuario> userManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<Usuario> ListNotAuthenticatedUsers { get; set; }

        private string statusMessage = String.Empty;

        private string messageType = String.Empty;

        private bool isLoading;

        protected override void OnInitialized()
        {
            ListNotAuthenticatedUsers = new List<Usuario>();
            isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            ListNotAuthenticatedUsers = (await userService.GetNotAuthenticatedUsers()).ToList();
            isLoading = false;
        }

        public async void resendEMailConfirmation(string userEmail)
        {
            isLoading = true;
            StateHasChanged();

            var user = (await userService.GetAllUsersWithDetailsAsync()).ToList().Find(u => u.Email == userEmail);

            var emailConfirmedToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmedToken));
            var firstHalf = ((int)code.Length / 2);
            var code1 = code.Substring(0, firstHalf);
            var code2 = code.Substring(firstHalf, code.Length - firstHalf);
            var emailCoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));
            var url = $"{NavigationManager.BaseUri}/validateUserAccount/" + emailCoded + "/" + code1 + "/" + code2;
            var message = new EmailContentModel()
            {
                Destination = user.Email,
                Subject = "PRIME@UCR: Validación nueva cuenta de usuario",
                Body = $"<p>Estimado(a) {user.Persona.Nombre}, se ha creado una cuenta a su nombre en la aplicación PRIME@UCR. Si desea validar su cuenta, para poder hacer uso de ella y sus funcionalidad, favor presionar <a href=\"{url}\">aquí</a>. En caso contrario, favor ignorar este mensaje. </p>"
            };

            await mailService.SendEmailAsync(message);
            statusMessage = "Se ha reenviado un correo de validación de cuenta al usuario indicado.";
            messageType = "success";
            isLoading = false;
            StateHasChanged();
            
        }
    }
}
