using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IDoctorService
    {
        Task<Médico> GetDoctorByIdAsync(string id);
        Task<IEnumerable<Médico>> GetAllDoctorsAsync();
    }
}