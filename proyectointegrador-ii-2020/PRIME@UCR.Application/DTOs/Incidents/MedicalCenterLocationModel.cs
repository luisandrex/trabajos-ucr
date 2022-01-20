using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Dtos.Incidents
{
    public class MedicalCenterLocationModel
    {
        public CentroMedico MedicalCenter { get; set; }
        public Médico Doctor { get; set; }
        public int? BedNumber { get; set; }
        public bool IsOrigin { get; set; }
    }
}