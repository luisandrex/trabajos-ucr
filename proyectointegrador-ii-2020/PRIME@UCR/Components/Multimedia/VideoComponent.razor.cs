using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Multimedia
{
    public partial class VideoComponent
    {

        // Reference to Multimedia Modal Parent
        [Parameter]
        public MultimediaModal MultimediaModal { get; set; }
        /* Appointment code for auto naming real time multimedia content.
         */
        [Parameter]
        public string ApCode { get; set; }

        // Element References
        ElementReference startButton;
        ElementReference videoPreview;
        ElementReference recordedVideo;
        ElementReference downloadButton;
        ElementReference stopButton;
        ElementReference closeButton;
        ElementReference mockDownloadButton;
        ElementReference downloadButtonContainer;

        string fileName = "";
        MAlertMessage AlertMessage;
        MAlertMessage OpenCameraAlertMessage;
        MAlertMessage PressRecordAlertMessage;
        MAlertMessage PressStopAlertMessage;
        MAlertMessage VideoProcessingMessage;
        MAlertMessage VideoReadyMessage;

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
                Message = "Abra la cámara para grabar video."
            };

            PressRecordAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Grabar para empezar a grabar video."
            };

            PressStopAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Detener para detener la grabación del video."
            };

            VideoProcessingMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Por favor, espere mientras se procesa el video."
            };

            VideoReadyMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Por favor, espere a que el video sea procesado y luego presione el " +
                "botón de Descargar para descargar el video o Grabar para grabar otro video."
            };

            AlertMessage = OpenCameraAlertMessage;
        }

        bool firstRender = true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // call JS code to initialize Element References
            if (firstRender)
                await JS.InvokeAsync<bool>("videoInit", startButton, videoPreview, recordedVideo, downloadButton, stopButton, closeButton, mockDownloadButton, downloadButtonContainer);
            firstRender = false;
        }

        async Task CloseComponent()
        {
            if (MultimediaModal != null)
                MultimediaModal.OnModalClosed -= CloseComponent;
        }

        // Open/Close Button
        bool cameraOpen = false;
        bool cameraClose => !cameraOpen;
        string openCloseButtonText => !cameraOpen ? "Abrir Cámara" : "Cerrar Cámara";

        async Task HandleOpenCloseClick()
        {
            if (!cameraOpen)
            {
                await JS.InvokeAsync<bool>("openCamera", videoPreview);
                AlertMessage = PressRecordAlertMessage;
            }
            else
            {
                await JS.InvokeAsync<bool>("stop", videoPreview, true);
                await JS.InvokeAsync<bool>("closeCamera", videoPreview);
                AlertMessage = OpenCameraAlertMessage;
            }
            cameraOpen = !cameraOpen;
        }

        async Task OnClose()
        {
            await JS.InvokeAsync<bool>("stop", videoPreview, true);
            MultimediaModal?.CloseImageView();
        }

        string GetFileName()
        {
            return "VID-" + ApCode + "-" + MultimediaContentComponent.FormatDate(DateTime.Now);
        }

        async Task UpdateFileName()
        {
            //await JS.InvokeAsync<bool>("updateImageDownloadName", downloadLinkRef, fileName);
            await JS.InvokeAsync<bool>("setDownloadName", fileName);
        }

        void OnRecordPressed()
        {
            AlertMessage = PressStopAlertMessage;
        }

        void OnStopPressed()
        {
            AlertMessage = VideoReadyMessage;
            // actualizar fileName
            fileName = GetFileName();
            UpdateFileName();
        }




    }
}
