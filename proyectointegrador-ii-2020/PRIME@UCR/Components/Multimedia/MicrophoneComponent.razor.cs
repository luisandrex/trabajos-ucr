using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Multimedia
{
    public partial class MicrophoneComponent
    {
        [Parameter]
        public MultimediaModal MultimediaModal { get; set; }
        /* Appointment code for auto naming real time multimedia content.
         */
        [Parameter]
        public string ApCode { get; set; }

        // Element References
        ElementReference recordButton;
        ElementReference stopButton;
        ElementReference audio;
        ElementReference timer;
        ElementReference downloadLink;
        ElementReference fillDiv;

        string fileName = "";
        MAlertMessage AlertMessage;
        MAlertMessage RecordAlertMessage;
        MAlertMessage StopAlertMessage;
        MAlertMessage SaveAudioMessageAlertMessage;

        bool recording = false;
        bool audioRecorded = false;
        string downloadLinkClass => !recording && audioRecorded ? "btn btn-primary rt-button" : "hidden";

        protected override void OnInitialized()
        {
            fileName = GetFileName();
            UpdateFileName();

            RecordAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Grabar para empezar a grabar el audio."
            };

            StopAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Detener para detener la grabación del audio."
            };

            SaveAudioMessageAlertMessage = new MAlertMessage
            {
                AlertType = AlertType.Primary,
                Message = "Presione el botón de Descargar para descargar el audio o Grabar para grabar" +
                " otro audio."
            };

            AlertMessage = RecordAlertMessage;
        }

        bool firstRender = true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // call JS code to initialize Element References
            if (firstRender)
                await JS.InvokeAsync<bool>("initAudio", recordButton, stopButton, audio, timer, downloadLink, fillDiv);
            firstRender = false;
        }

        void OnClose()
        {
            MultimediaModal?.CloseImageView();
        }

        void OnRecord()
        {
            AlertMessage = StopAlertMessage;
            recording = true;
        }

        void OnStop()
        {
            AlertMessage = SaveAudioMessageAlertMessage;
            fileName = GetFileName();
            UpdateFileName();
            recording = false;
            audioRecorded = true;
        }


        string GetFileName()
        {
            return "AUD-" + ApCode + "-" + MultimediaContentComponent.FormatDate(DateTime.Now);
        }

        async Task UpdateFileName()
        {
            await JS.InvokeAsync<bool>("updateAudioName", fileName);
        }


    }
}
