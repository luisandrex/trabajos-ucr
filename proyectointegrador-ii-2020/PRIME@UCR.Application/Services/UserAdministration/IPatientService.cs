using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IPatientService
    {
        Task<Paciente> GetPatientByIdAsync(string id);
        Task<Paciente> CreatePatientAsync(Paciente entity);
        Task<Paciente> InsertPatientOnlyAsync(Paciente entity);
    }
}