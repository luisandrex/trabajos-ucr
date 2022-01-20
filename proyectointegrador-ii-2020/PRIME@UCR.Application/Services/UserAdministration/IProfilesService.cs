using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Threading.Tasks;


namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IProfilesService
    {
        Task<List<Perfil>> GetPerfilesWithDetailsAsync();
        Task<bool> IsAdministratorAsync(string id);
        Task<bool> IsCoordinatorAsync(string id);
        Task<bool> IsDoctorAsync(string id);
        Task<bool> IsTechnicalSpecialistAsync(string id);
    }
}
