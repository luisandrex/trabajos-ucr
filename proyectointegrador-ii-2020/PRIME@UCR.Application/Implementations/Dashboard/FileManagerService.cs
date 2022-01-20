using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.Dashboard
{
    
    public class FileManagerService : IFileManagerService
    {
        private readonly IFileService fileService;
        private readonly IMailService mailService;
        private readonly Random _random;
        public FileManagerService(IFileService _fileService, IMailService _mailService)
        {
            fileService = _fileService;
            mailService = _mailService;
            _random = new Random();
        }

        public async Task createFileAsync(List<Incidente> filteredIncidentsData, string userIdentifier, string userName )
        {
            List<IncidentListModel> listaIncidentes = new List<IncidentListModel>();
            var differentiator = RandomString(10);
           
            string container = "";
            string fileName = userIdentifier + differentiator + ".csv";
            container += "Código,Origen,Destino,Modalidad,Fecha y hora de registro,Fecha y hora de registro,Estado,Id Destino\n";

            foreach (Incidente incident in filteredIncidentsData)
            {
                container += incident.Codigo + "," + incident.Origen?.GetType().Name + "," 
                            + incident.Destino?.GetType().Name + "," + incident.Modalidad + "," 
                            +  incident.Cita.FechaHoraCreacion  +"," + incident.Cita.FechaHoraEstimada +"," 
                            + incident.EstadoIncidentes.ToArray()[incident.EstadoIncidentes.Count - 1].NombreEstado + ","
                            + incident.IdDestino + "\n";
            }
            string path = await fileService.StoreTextFile(container, fileName);

            var message = new EmailContentModel()
            {
                Destination = userIdentifier,
                Subject = "PRIME@UCR: Solicitud de lista de incidentes",
                Body = $"<p>Estimado(a) {userName}, usted ha solicitado la generación un archivo .csv con la lista de incidentes en la aplicación PRIME@UCR.</p>",
                AttachmentPath = path
            };

            await mailService.SendEmailAsync(message);
            fileService.DeleteFile(path);
        }

        private string RandomString(int size)
        {
            var builder = new StringBuilder(size);

            
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            var offset = 'a';
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            var returnValue = builder.ToString();
            return returnValue;
        }
    }

}
