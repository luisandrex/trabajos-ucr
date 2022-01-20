using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PRIME_UCR.Components.Multimedia
{

    public partial class CameraComponent
    {
        // Reference to Multimedia Modal Parent
        [Parameter]
        public MultimediaModal MultimediaModal { get; set; }

        // Element References
        ElementReference videoElement;
        ElementReference canvasElement;
        ElementReference imageElement;
        ElementReference downloadLinkRef;

        // State Variables
        bool cameraOpen = false;
        bool cameraClose => !cameraOpen;
        bool photoTaken = false;
        bool photoNotTaken => !photoTaken;
        // Class Variables
        string videoClass => !photoTaken ? "rt-box" : "hidden";
        string canvasClass => photoTaken ? "rt-box" : "hidden";
        string tpButtonClass => !photoTaken ? "btn btn-primary rt-button" : "hidden"; // Take Photograph Button Class
        string cancelButtonClass => photoTaken ? "btn btn-danger rt-button" : "hidden";
        string downloadLinkClass => photoTaken ? "btn btn-primary rt-button" : "hidden";
        // Valid file name Indicator
        bool validTitle = false;
        bool notValidSave =>  photoNotTaken || !validTitle;

        string fileName = "";
        MAlertMessage AlertMessage;
        MAlertMessage OpenCameraAlertMessage;
        MAlertMessage PressTakePhotoAlertMessage;
        MAlertMessage PhotoTakenAlertMessage;

        /* Appointment code for auto naming real time multimedia content.
         */
        [Parameter]
        public string ApCode { get; set; }

        protected override void OnInitialized()
        {
            // add CloseComponent method to OnModalClosed event
            if (MultimediaModal != null)
                MultimediaModal.OnModalClosed += CloseComponent;

            fileName = GetFileName();
            UpdateFileName();


            OpenCameraAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Abra la cámara para tomar fotografía."
            };

            PressTakePhotoAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presionar el botón de Tomar Fotografía para tomar fotografía."
            };

            PhotoTakenAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Descargar para descargar la fotografía o Volver a tomar para tomar " +
                "otra fotografía."
            };

            AlertMessage = OpenCameraAlertMessage;
        }

        // Open Close Camera Button Code
        async Task HandleOpenCloseButtonClick()
        {
            if (!cameraOpen) await OpenCamera();
            else await CloseCamera();
        }
        async Task OpenCamera()
        {
            await JS.InvokeAsync<bool>("openCamera", videoElement);
            cameraOpen = true;
            AlertMessage = PressTakePhotoAlertMessage;
        }
        async Task CloseCamera()
        {
            await JS.InvokeAsync<bool>("closeCamera", videoElement);
            cameraOpen = false;
            AlertMessage = OpenCameraAlertMessage;
        }
        string OpenCloseButtonText()
        {
            return !cameraOpen ? "Abrir Cámara" : "Cerrar Cámara";
        }

        async Task TakePhotograph()
        {
            await JS.InvokeAsync<string>("takePhotograph", canvasElement, videoElement, imageElement, downloadLinkRef);
            photoTaken = true;
            AlertMessage = PhotoTakenAlertMessage;
        }
        async Task CancelPhotograph()
        {
            photoTaken = false;
            await JS.InvokeAsync<bool>("clearCanvas", canvasElement);
            fileName = GetFileName();
            UpdateFileName();
            AlertMessage = PressTakePhotoAlertMessage;

        }
        async Task CloseComponent()
        {
            await CloseCamera();
            if (MultimediaModal != null)
                MultimediaModal.OnModalClosed -= CloseComponent;
        }
        async Task OnClose()
        {
            MultimediaModal?.CloseImageView();
        }

        string GetFileName()
        {
            return "IMG-" + ApCode + "-" + MultimediaContentComponent.FormatDate(DateTime.Now);
        }

        async Task UpdateFileName()
        {
            await JS.InvokeAsync<bool>("updateImageDownloadName", downloadLinkRef, fileName);
        }


    }
}
