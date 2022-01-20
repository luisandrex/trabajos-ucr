using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Multimedia
{
    public partial class TextComponent
    {
        [Inject]
        protected IFileService fileService { get; set; }
        /* Appointment code for auto naming real time multimedia content.
         */
        [Parameter]
        public string ApCode { get; set; }
        [Parameter]
        public string ActionName { get; set; } = null;
        [Parameter]
        public string CallingPlace { get; set; } = "";
        [Parameter]
        public string CheckListName { get; set; } = null;
        [Parameter]
        public string CheckListItemName { get; set; } = null;

        [Inject]
        public IEncryptionService EncryptionService { get; set; }
        [Inject]
        protected IMultimediaContentService mcService { get; set; }
        [Parameter]
        public MultimediaModal MultimediaModal { get; set; }
        [Parameter]
        public MultimediaContent MultimediaContent { get; set; }
        /* Function pass as parameter from Parent Component to be notified
         * when a file has been uploaded. 
         */
        [Parameter]
        public EventCallback<MultimediaContent> OnFileUpload { get; set; }
        

        string fileName; //input file name
        string text; // input file text
        bool viewMode => MultimediaContent != null;

        MAlertMessage AlertMessage;
        MAlertMessage PressSaveAlertMessage;
        MAlertMessage PressNewAlertMessage;

        protected override void OnInitialized()
        {
            fileName = GetFileName();

            PressSaveAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Guardar después de escribir la nota para adjuntar " +
                "el archivo."
            };

            PressNewAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón Nueva Nota para escribir una nueva nota."
            };

            AlertMessage = PressSaveAlertMessage;
        }

        protected override async Task OnInitializedAsync()
        {
            if (viewMode)
            {
                string nameDecoded = encrypt_service.DecodeString(MultimediaContent.Nombre);
                byte[] nameDecodedByte = Convert.FromBase64String(nameDecoded);
                fileName = encrypt_service.Decrypt(nameDecodedByte);
                text = GetMCText();
            }
        }

        string GetMCText()
        {
            string textContent = "";
            string pathEncrypted = MultimediaContent.Archivo;
            byte[] pathEncryptedByte = System.Convert.FromBase64String(pathEncrypted);
            string pathDecrypted = EncryptionService.Decrypt(pathEncryptedByte);
            EncryptionService.DecryptFile(pathDecrypted);
            textContent = System.IO.File.ReadAllText(pathDecrypted);
            return textContent;
        }

        // Method to store text file
        public async Task StoreTextFile()
        {
            byte[] encryptedByte = EncryptionService.Encrypt(fileName);
            string justName = EncryptionService.EncodeString(Convert.ToBase64String(encryptedByte));
            string path = await fileService.StoreTextFile(text, justName, ApCode, CallingPlace, ActionName, CheckListName, CheckListItemName);
            byte[] epArray = EncryptionService.Encrypt(path);
            string encryptedPath = Convert.ToBase64String(epArray);
            MultimediaContent mc = new MultimediaContent
            {
                Nombre = justName,
                Archivo = encryptedPath,
                Descripcion = "",
                Fecha_Hora = DateTime.Now,
                Tipo = "text/plain",
                Extension = ".txt"
            };

            mc = await mcService.AddMultimediaContent(mc); // add MC to DB
            await OnFileUpload.InvokeAsync(mc); // invoke callback function
        }

        void OnClose()
        {
            MultimediaModal?.CloseImageView();
        }


        string GetFileName()
        {
            return "NOTA-" + ApCode + "-" + MultimediaContentComponent.FormatDate(DateTime.Now);
        }

    }
}
