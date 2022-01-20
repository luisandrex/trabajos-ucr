using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.DTOs.CheckLists
{
    public class PdfModel
    {
        public IncidentDetailsModel Incident { get; set; }
        public List<PadecimientosCronicos> ChronicConditions { get; set; }
        public List<Antecedentes> Background { get; set; }
        public AssignmentModel AssignedMembers { get; set; }
        public Paciente Patient { get; set; }
        public List<TipoAccion> ActionTypes { get; set; }
        public List<List<MultimediaContent>> ExistingFiles { get; set; }
    }
}
