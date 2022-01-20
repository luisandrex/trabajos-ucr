using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class RegisterUser
    {
        [Inject]
        public IPersonService personService { get; set; } 

        [Inject]
        public IUserService userService { get; set; }

        [Inject]
        public INumeroTelefonoService telefonoService { get; set; }

        [Inject]
        public IPerteneceService perteneceService { get; set; }
        
        [Inject]
        public IMailService mailService { get; set; }

        [Inject]
        public UserManager<Usuario> userManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string statusMessage;

        private string messageType;

        public RegisterUserFormModel infoOfUserToRegister;

        private bool isBusy;

        protected override void OnInitialized()
        {
            infoOfUserToRegister = new RegisterUserFormModel();
            isBusy = false;
        }

        /**
         * Method used to register a user once the admin fill the form.
         */
        private async void RegisterUserInDB()
        {
            isBusy = true;
            StateHasChanged();
            var personModel = await personService.GetPersonModelFromRegisterModelAsync(infoOfUserToRegister);
            var existPersonInDB = (await personService.GetPersonByCedAsync(personModel.IdCardNumber)) == null ? false : true;
            if (!existPersonInDB)
            {
                await personService.StoreNewPersonAsync(personModel);
                var userModel = await userService.GetUserFormFromRegisterUserFormAsync(infoOfUserToRegister);
                var tempPassword = personModel.Name + "." + personModel.FirstLastName + personModel.PrimaryPhoneNumber;/*Es temporal, luego esto cambiará*/
                var result = await userService.StoreUserAsync(userModel);
                if(!result)
                {
                    await personService.DeletePersonAsync(userModel.IdCardNumber);
                    statusMessage = "El usuario indicado ya forma parte de la aplicación.";
                    messageType = "danger";
                } else
                {
                    try
                    {
                        await telefonoService.AddNewPhoneNumberAsync(personModel.IdCardNumber, infoOfUserToRegister.PrimaryPhoneNumber);
                        if (!String.IsNullOrEmpty(infoOfUserToRegister.SecondaryPhoneNumber))
                        {
                            await telefonoService.AddNewPhoneNumberAsync(personModel.IdCardNumber, infoOfUserToRegister.SecondaryPhoneNumber);
                        }
                    }
                    catch (Exception)
                    {
                        //Nothing happens phone number duplicated
                    }
                   

                    var user = (await userService.GetAllUsersWithDetailsAsync()).ToList().Find(u => u.Email == userModel.Email);

                    foreach (String profileName in infoOfUserToRegister.Profiles)
                    {
                        await perteneceService.InsertUserOfProfileAsync(user.Id, profileName);
                    }
                    infoOfUserToRegister = new RegisterUserFormModel();
                    var emailConfirmedToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmedToken));
                    var firstHalf = ((int)code.Length / 2);
                    var code1 = code.Substring(0, firstHalf);
                    var code2 = code.Substring(firstHalf, code.Length - firstHalf);
                    var emailCoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));
                    var url = $"{NavigationManager.BaseUri}/validateUserAccount/" + emailCoded + "/" + code1 + "/" + code2;
                    var message = new EmailContentModel()
                    {
                        Destination = userModel.Email,
                        Subject = "PRIME@UCR: Validación nueva cuenta de usuario",
                        Body = $"<p>Estimado(a) {user.Persona.Nombre}, se ha creado una cuenta a su nombre en la aplicación PRIME@UCR. Si desea validar su cuenta, para poder hacer uso de ella y sus funcionalidades, favor presionar <a href=\"{url}\">aquí</a>. En caso contrario, favor ignorar este correo. </p>"
                    };

                    await mailService.SendEmailAsync(message);
                    statusMessage = "El usuario indicado se ha registrado en la aplicación y se le ha enviado un correo de validación de cuenta.";
                    messageType = "success";
                }
            } else
            {
                statusMessage = "El usuario indicado ya forma parte de la aplicación.";
                messageType = "danger";
            }
            isBusy = false;
            StateHasChanged();
        }
    }
}
