using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IMedicalCenterRepository : IGenericRepository<CentroMedico, int>
    {
        Task<IEnumerable<Médico>> GetDoctorsByMedicalCenterId(int medicalCenterId);


    }
}