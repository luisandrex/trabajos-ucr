using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Multimedia
{
    public partial class MultimediaContentComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        // The List of Multimedia Content displayed by the component
        [Parameter]
        public List<MultimediaContent> MultimediaContent { get; set; }
        /* Function pass as parameter from Parent Component to be notified
         * when a file has been uploaded. 
         */
        [Parameter]
        public EventCallback<MultimediaContent> OnFileUpload { get; set; }
        /* Parameter that indicates if the component is view only or
         * if it accepts changes.
         */
        [Parameter]
        public bool ViewOnly { get; set; } = false;
        /* Appointment code for auto naming real time multimedia content.
         */
        [Parameter]
        public string ApCode { get; set; } = "";
        [Parameter]
        public string ActionName { get; set; } = null;
        [Parameter]
        public string CallingPlace { get; set; } = "";
        [Parameter]
        public string CheckListName { get; set; } = null;
        [Parameter]
        public string CheckListItemName { get; set; } = null;

        
        // List of valid file types 
        public List<string> validTypeFiles;
        bool validFileType = true;

        bool open = false; // State of the dropdown 
        string divDDClass = "dropdown";
        string ddMenuClass = "dropdown-menu dropdown-menu-right";
        string invalidMessage = "";

        // Modal Variables 
        bool showModal = false;
        bool showCamera = false;
        bool showMicrophone = false;
        bool showAudio = false;
        bool showImage = false;
        bool showText = false;
        bool showVideo = false;
        bool showVideoComponent = false;
        bool showTextComponent = false;
        bool showPDF = false;
        bool showDropdown = false;
        MultimediaContent modalMContent = null;
         
        protected override void OnInitialized()
        {
            validTypeFiles = new List<string>() { "ogg", "oga", "jpeg", "webm", "mpeg", "pdf", "doc", "docx", "xls", "txt", "mp3", "jpg", "png", "mp4", "wmv", "avi", "text/plain" };
        }
        //@Parametros: Ninguno
        //@Funcion: Controla que el dropdown de los archivos se abra y cierre correctamente
        //@Retorno: Ninguno
        public void Open()
        {
            divDDClass = open ? "dropdown" : "dropdown show";
            ddMenuClass = open ? "dropdown-menu dropdown-menu-right" : "dropdown-menu dropdown-menu-right show";

            open = !open;
        }
        //@Parametros: Ninguno
        //@Funcion: Genera la direccion en la cual se va a guardar el archivo (segun la estructura del repositorio)
        //@Retorno: La direccion del directorio donde se guardara el archivo
        string getFilePath() {
            string multiString = "Multimedia";
            byte[] multiByte = encrypt_service.Encrypt(multiString);
            string multiEncrypted = Convert.ToBase64String(multiByte);
            string path = "wwwroot/data/"+ encrypt_service.EncodeString(multiEncrypted)+"/";
            byte[] IncidentCodeEncryptedByte = null;
            string IncidentCodeEncryptedString = "";
            string general = "General";
            switch (CallingPlace){
                case "action":
                    IncidentCodeEncryptedByte = encrypt_service.Encrypt(ApCode);
                    IncidentCodeEncryptedString = Convert.ToBase64String(IncidentCodeEncryptedByte);
                    path += encrypt_service.EncodeString(IncidentCodeEncryptedString);
                    path += "/";
                    byte[] ActionNameEncryptedByte = encrypt_service.Encrypt(ActionName);
                    string ActionNameEncryptedString = Convert.ToBase64String(ActionNameEncryptedByte);
                    path += encrypt_service.EncodeString(ActionNameEncryptedString);
                    path += "/";
                break;
                case "checkListItem":
                    IncidentCodeEncryptedByte = encrypt_service.Encrypt(ApCode);
                    IncidentCodeEncryptedString = Convert.ToBase64String(IncidentCodeEncryptedByte);
                    path += encrypt_service.EncodeString(IncidentCodeEncryptedString);
                    path += "/";
                    byte[] checkListNameByte = encrypt_service.Encrypt(CheckListName);
                    string checkListNameEncryptedString = Convert.ToBase64String(checkListNameByte);
                    path += encrypt_service.EncodeString(checkListNameEncryptedString);
                    path += "/";
                    byte[] checkListItemByte = encrypt_service.Encrypt(CheckListItemName);
                    string checkListItemString = Convert.ToBase64String(checkListItemByte);
                    path += encrypt_service.EncodeString(checkListItemString);
                    path += "/";
                break;
                default:
                    byte[] generalRepositoyByte = encrypt_service.Encrypt(general);
                    string generalRepositoyByteString = Convert.ToBase64String(generalRepositoyByte);
                    path += encrypt_service.EncodeString(generalRepositoyByteString);
                    path += "/";
                    break;
            }
            return path;
    }
        //@Parametros: Un array de archivos fisicos
        //@Funcion: Elige el primer archivo del array y hace todas las operaciones sobre este para almacenarlo en la base 
        //          y en el directorio correspondiente dentro del servidor
        //@Retorno: La tarea asincronica
        async Task HandleFileSelected(IFileListEntry[] files)
        {
            IFileListEntry file = files.FirstOrDefault();

            validFileType = ValidateFile(file);

            if (!validFileType) return; 

            MultimediaContent mcontent = await StoreMultimediaContent(file);
            await file_service.StoreFile(getFilePath(), mcontent.Nombre,mcontent.Extension, file.Data);
            await OnFileUpload.InvokeAsync(mcontent);
        }
        //@Parametros: El nombre completo del archivo, ejemplo: "prueba.pdf"
        //@Funcion: Extrae la extension del nombre del archivo
        //@Retorno: Un string que contiene la extension del archivo
        string getType(string filename) {
            string extension = ".";
            foreach (string type in validTypeFiles) {
                if (filename.Contains(type)) {
                    extension += type;
                }
            }
            return extension;
        }
        //@Parametros: El archivo adjuntado
        //@Funcion: Genera un MultimediaContent con la informacion del archivo (llamando al metodo FileToMultimediaContent) y 
        //          llama al servicio multimedia para guardar la informacion en la base de datos
        //@Retorno: La tarea asincronica
        async Task<MultimediaContent> StoreMultimediaContent(IFileListEntry file)
        {
            if (file == null) return null;
            MultimediaContent mcontent = FileToMultimediaContent(file, getFilePath());
            return await multimedia_content_service.AddMultimediaContent(mcontent);
        }
        //@Parametros: El archivo adjuntado, el path fisico de ese archivo
        //@Funcion: A partir de un archivo fisico, crea un MultimediaContent con la informacion del mismo
        //@Retorno: El MultimediaContent con la informacion del archivo
        MultimediaContent FileToMultimediaContent(IFileListEntry file, string pathDecrypted)
        {
            string justName = file.Name.Replace(getType(file.Name),"");
            byte[] fileNameEncrytedByte = encrypt_service.Encrypt(justName);
            string fileNameEncrytedString = Convert.ToBase64String(fileNameEncrytedByte);
            string filename = encrypt_service.EncodeString(fileNameEncrytedString);
            string path = pathDecrypted + filename + getType(file.Name);
            byte[] pathEncryptedByte = encrypt_service.Encrypt(path);
            string pathEncryptedString = Convert.ToBase64String(pathEncryptedByte);
            return new MultimediaContent
            {
                Nombre = filename,
                Archivo = pathEncryptedString,
                Descripcion = "",
                Fecha_Hora = DateTime.Now,
                Tipo = file.Type,
                Extension = getType(file.Name)
            };
        }
        //@Parametros: El archivo que se quiere adjuntar
        //@Funcion: Valida el archivo para evitar que se adjunten archivos no permitidos o vacios
        //@Retorno: Un booleano que indica si es válido o no
        bool ValidateFile(IFileListEntry file)
        {
            if (file == null) return false;

            string type = file.Type;

            foreach (string validType in validTypeFiles)
            {
                bool b1 = type.Contains(validType);
                bool b2 = type == validType;
                if (b1 || b2) return true;
            }

            return false;
        }
        //@Parametros: El multimedia content que contiene la infomacion del archivo
        //@Funcion: Filtra que tipo de archivo se desea mostrar y llama al metodo encargado de establecer la vista
        //@Retorno: La tarea asincronica
        async Task ShowPopUp(MultimediaContent mcontent) {
            string name = mcontent.Nombre; 
            string pathEncrypted = mcontent.Archivo;
            string extension = mcontent.Extension;
            if (extension == ".png" || extension == ".jpeg" || extension == ".jpg")
                OpenImage(mcontent);
            else if (extension == ".txt")
                OpenText(mcontent);
            else if (extension == ".mp4" || extension == ".webm" || extension ==".avi" || extension==".mp4avi")
                OpenVideo(mcontent);
            else if (extension == ".mp3" || extension == ".ogg" || extension == ".oga")
                OpenAudio(mcontent);
            else if (extension == ".pdf")
                OpenPDF(mcontent);
        }
        //@Parametros: Ninguno
        //@Funcion: Muestra un mensaje de error al usuario debido al tipo de archivo ingresado
        //@Retorno: El mensaje de error con los tipos de dato que acepta la aplicacion
        string InvalidTypeMessage()
        {
            string datas = "El archivo seleccionado no se encuentra dentro de los archivos válidos. Por favor seleccione un archivo con las siguientes extensiones: ";
            for (int i = 0; i < validTypeFiles.Count(); ++i)
            {
                datas += validTypeFiles[i];
                if (i < (validTypeFiles.Count() - 1))
                {
                    datas += ",";
                }
            }
            return datas;
        }
        //@Parametros: El multimedia content que tiene la informacion del archivo
        //@Funcion: Cambia el texto del botón "Ver" a "Escuchar" si es un audio
        //@Retorno: El nombre que va a tener el botón
        string GetButonName(MultimediaContent mcontent) {
            string name = "";
            if (mcontent.Tipo == "audio/mpeg" || mcontent.Tipo == "audio/ogg") {
                name = "Escuchar";
            }
            else {
                name = "Ver"; 
            }
            return name; 
        }
        //@Parametros: El multimedia content que tiene la información del archivo
        //@Funcion: Decodifica y desencripta el nombre del archivo para mostrarlo en la vista del usuario
        //@Retorno: El nombre completo del archivo
        string GetFileName(MultimediaContent mcontent) {
            string decodedName = encrypt_service.DecodeString(mcontent.Nombre);
            byte[] decodedNameByte = Convert.FromBase64String(decodedName);
            string name = encrypt_service.Decrypt(decodedNameByte);
            name += mcontent.Extension;
            return name;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre la Camara
        //@Retorno: Ninguno
        void OpenCamera(){
            showModal = true;
             showCamera = true;
             showMicrophone = false;
             showAudio = false;
             showImage = false;
             showText = false;
             showVideo = false;
             showVideoComponent = false;
             showTextComponent = false;
             showPDF = false;
             modalMContent = null;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre un audio
        //@Retorno: Ninguno
        void OpenAudio(MultimediaContent mcontent){
            showModal = true;
            showCamera = false;
            showMicrophone = false;
            showAudio = true;
            showImage = false;
            showText = false;
            showVideo = false;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = false;
            modalMContent = mcontent;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre la imagen
        //@Retorno: Ninguno
        void OpenImage(MultimediaContent mcontent){
            showModal = true;
            showCamera = false;
            showMicrophone = false;
            showAudio = false;
            showImage = true;
            showText = false;
            showVideo = false;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = false;
            modalMContent = mcontent;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre el texto
        //@Retorno: Ninguno
        void OpenText(MultimediaContent mcontent) {
            showModal = true;
            showCamera = false;
            showMicrophone = false;
            showAudio = false;
            showImage = false;
            showText = true;
            showVideo = false;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = false;
            modalMContent = mcontent;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre la Camara para capturar video
        //@Retorno: Ninguno
        void OpenVideo(MultimediaContent mcontent) {
            showModal = true;
            showCamera = false;
            showMicrophone = false;
            showAudio = false;
            showImage = false;
            showText = false;
            showVideo = true;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = false;
            modalMContent = mcontent;
        }
        //@Parametros: Ninguno
        //@Funcion: Activa el evento necesario para que se muestre el video
        //@Retorno: Ninguno
        void OpenVideo(){
            showModal = true;
            showCamera = false;
            showMicrophone = false;
            showAudio = false;
            showImage = false;
            showText = false;
            showVideo = false;
            showVideoComponent = true;
            showTextComponent = false;
            showPDF = false;
            modalMContent = null;
        }
        //@Parametros: Ninguno
        //@Funcion: Activa el evento necesario para que se muestre el microfono
        //@Retorno: Ninguno
        void OpenMicrophone(){
            showModal = true;
            showCamera = false;
            showMicrophone = true;
            showAudio = false;
            showImage = false;
            showText = false;
            showVideo = false;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = false;
            modalMContent = null;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Activa el evento necesario para que se muestre el pdf
        //@Retorno: Ninguno
        void OpenPDF(MultimediaContent mcontent) {
            showModal = false;
            showCamera = false;
            showMicrophone = false;
            showAudio = false;
            showImage = false;
            showText = false;
            showVideo = false;
            showVideoComponent = false;
            showTextComponent = false;
            showPDF = true;
            modalMContent = mcontent;
        }
        //@Parametros: Ninguno
        //@Funcion: Cierra todas las vistas de archivos
        //@Retorno: Ninguno
        void CloseAllViews() {
            showModal = false;
            showPDF = false;
        }
        //@Parametros: El MultimediaContent que contiene la informacion del archivo
        //@Funcion: Borra el archivo seleccionado de la base de datos y fisicamente
        //@Retorno: La tarea asincronica
        async Task DeleteMultimediaContent(MultimediaContent mcontent){
            CloseAllViews();
            await multimedia_content_service.DeleteMultimediaContent(mcontent);
            MultimediaContent.Remove(mcontent);
            byte[] bEPath = Convert.FromBase64String(mcontent.Archivo);
            string path = encrypt_service.Decrypt(bEPath);
            file_service.DeleteFile(path);
        }

        public static string FormatDate(DateTime dateTime)
        {
            string sDateTime = dateTime.ToString();
            sDateTime = sDateTime.Replace('/', '-'); // replace slashes in date
            sDateTime = sDateTime.Replace(' ', '-'); // replace space between date and time
            sDateTime = sDateTime.Replace(':', '-'); // replace double dots in time
            return sDateTime;
        }


    }
}
